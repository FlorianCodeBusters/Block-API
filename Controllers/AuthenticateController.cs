using Blocks_api.Dtos;
using Blocks_api.Extensions;
using Blocks_api.Logger;
using Blocks_api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Blocks_api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthenticateController : ControllerBase
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly IBlockLogger _logger;

        public AuthenticateController(IAuthenticationService authenticationService, IBlockLogger logger)
        {
            _authenticationService = authenticationService;
            _logger = logger;
        }

        [AllowAnonymous]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultDto<string>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ResultDto<string>))]
        public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
        {
            var response = await _authenticationService.Login(loginRequest);
            var resultDto = response.ToResultDto();
            if (!resultDto.IsSuccess)
            {
                _logger.Error($"Failed to authenticate user {loginRequest.Username}");
                return BadRequest(resultDto);
            }
            _logger.Info($"Successfully logged in user {loginRequest.Username}");
            return Ok(resultDto);
        }

        [AllowAnonymous]
        [HttpPost("register")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultDto<string>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ResultDto<string>))]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            var response = await _authenticationService.Register(request);
            var resultDto = response.ToResultDto();
            if (!resultDto.IsSuccess)
            {
                _logger.Error($"Failed to register {request.Username}");
                return BadRequest(resultDto);
            }
            _logger.Info($"Successfully registered {request.Username}");
            return Ok(resultDto);
        }
    }
}
