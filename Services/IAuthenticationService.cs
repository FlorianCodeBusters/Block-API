using Blocks_api.Dtos;
using FluentResults;

namespace Blocks_api.Services
{
    public interface IAuthenticationService
    {
        Task<Result<string>> Register(RegisterRequest registerRequest);
        Task<Result<string>> Login(LoginRequest loginRequest);
    }
}
