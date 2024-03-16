using Blocks_api.Dtos;

namespace Blocks_api.Services
{
    public interface IAuthenticationService
    {
        Task<string> Register(RegisterRequest registerRequest);
        Task<string> Login(LoginRequest loginRequest);
    }
}
