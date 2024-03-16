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

        public async Task<string> Register(RegisterRequest registerRequest)
        {
            User userByMail = await _userManager.FindByEmailAsync(registerRequest.Email);
            User userByUsername = await _userManager.FindByNameAsync(registerRequest.Username);

            if(userByMail != null)
            {
                throw new ArgumentException($"User with {userByMail} already exists");
            }else if (userByUsername != null)
            {
                throw new ArgumentException($"User with {userByUsername} already exists");
            }

            User newUser = new User()
            {
                Email = registerRequest.Email,
                UserName = registerRequest.Username,
                SecurityStamp = Guid.NewGuid().ToString(),
            };

            IdentityResult creationResult = await _userManager.CreateAsync(newUser, registerRequest.Password);

            if (!creationResult.Succeeded)
            {
                throw new ArgumentException($"Unable to register user {registerRequest.Username} errors: {GetErrorsText(creationResult.Errors)}");
            }

            return await Login(new LoginRequest { Username = registerRequest.Email, Password = registerRequest.Password });
        }

        public async Task<string> Login(LoginRequest loginRequest)
        {
            User user = await _userManager.FindByEmailAsync(loginRequest.Username);
            if (user == null)
            {
                user = await _userManager.FindByNameAsync(loginRequest.Username);
            }
            if(user == null || !await _userManager.CheckPasswordAsync(user, loginRequest.Password))
            {
                throw new ArgumentException($"Unable to authenticate user {loginRequest.Username}");
            }

            List<Claim> authClaims = new List<Claim>
            {
                new(ClaimTypes.Name, user.UserName),
                new(ClaimTypes.Email, user.Email),
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };

            var token = GetToken(authClaims);

            return new JwtSecurityTokenHandler().WriteToken(token);
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
