using Azure.Core;
using Blocks_api.Dtos;
using Blocks_api.Entities;
using Blocks_api.Models;
using System.Security.Claims;

using Microsoft.AspNetCore.Identity;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Security.Cryptography;
using FluentResults;

namespace Blocks_api.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly UserManager<User> _userManager;
        private readonly IConfiguration _configuration;

        public AuthenticationService(UserManager<User> userManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _configuration = configuration;
        }

        public async Task<Result<string>> Register(RegisterRequest registerRequest)
        {
            User userByMail = await _userManager.FindByEmailAsync(registerRequest.Email);
            User userByUsername = await _userManager.FindByNameAsync(registerRequest.Username);

            if(userByMail != null)
            {
                return Result.Fail(new Error($"User with {userByMail} already exists"));
            }else if (userByUsername != null)
            {
                return Result.Fail(new Error($"User with {userByUsername} already exists"));
            }

            User newUser = new User()
            {
                Email = registerRequest.Email,
                UserName = registerRequest.Username,
                SecurityStamp = Guid.NewGuid().ToString(),
            };

            IdentityResult creationResult = await _userManager.CreateAsync(newUser, registerRequest.Password);
            await _userManager.AddToRoleAsync(newUser, Roles.User);

            if (!creationResult.Succeeded)
            {
                return Result.Fail(new Error($"Unable to register user {registerRequest.Username} errors: {GetErrorsText(creationResult.Errors)}")); 
            }

            return await Login(new LoginRequest { Username = registerRequest.Email, Password = registerRequest.Password });
        }

        public async Task<Result<string>> Login(LoginRequest loginRequest)
        {
            User user = await _userManager.FindByEmailAsync(loginRequest.Username);
            if (user == null)
            {
                user = await _userManager.FindByNameAsync(loginRequest.Username);
            }
            if(user == null || !await _userManager.CheckPasswordAsync(user, loginRequest.Password))
            {
                return Result.Fail(new Error($"Unable to authenticate user {loginRequest.Username}"));
            }

            var userRoles = await _userManager.GetRolesAsync(user);

            List<Claim> authClaims = new List<Claim>
            {
                new(ClaimTypes.Name, user.UserName),
                new(ClaimTypes.Email, user.Email),
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };

            authClaims.AddRange(userRoles.Select(role => new Claim(ClaimTypes.Role, role)));

            var token = GetToken(authClaims);

            return Result.Ok(new JwtSecurityTokenHandler().WriteToken(token));
        }

        public static string HashPassword(string password)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                // Convert the input string to a byte array and compute the hash.
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(password));

                // Convert byte array to a string
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }

        private JwtSecurityToken GetToken(IEnumerable<Claim> authClaim)
        {

            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:ValidIssuer"],
                audience: _configuration["JWT:ValidAudience"],
                expires: DateTime.Now.AddHours(3),
                claims: authClaim,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256));
            return token;
        }

        private string GetErrorsText(IEnumerable<IdentityError> errors)
        {
            return string.Join(", ", errors.Select(error => error.Description).ToArray());
        }
    }
}
