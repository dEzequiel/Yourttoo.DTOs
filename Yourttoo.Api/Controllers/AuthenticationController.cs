using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Yourttoo.Api.Services;
using Yourttoo.DTOs.DTOs.Authentication;
using Yourttoo.DTOs.Requests.Authentication;
using Yourttoo.DTOs.Shared.API;

namespace Yourttoo.Api.Controllers
{
    [Route("/store/authentication")]
    [ApiController]
    [AllowAnonymous]
    public class AuthenticationController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ILogger<AuthenticationController> _logger;

            public AuthenticationController(IUserService userService, ILogger<AuthenticationController> logger)
        {
            _userService = userService;
            _logger = logger;
        }
        [HttpPost("login")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(LoginDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Login([FromBody] CreateLoginRequest request)
        {
            _logger.LogInformation("AuthenticationController --> Login --> Start for email: {Email}", request.Email);

            try
            {
                // Buscar usuario por email
                var user = await _userService.GetUserByEmailAsync(request.Email);

                if (user == null)
                {
                    _logger.LogWarning("Login failed for email: {Email} - User not found", request.Email);
                    return Unauthorized(new ApiResponse<string>("Credenciales inválidas"));
                    _logger.LogWarning("Login failed for email: {Email} - User not found", request.Email);
                    return Unauthorized(new ApiResponse<string>("Credenciales inválidas"));
                }

                _logger.LogInformation("Login successful for email: {Email}", request.Email);
                return Ok(new ApiResponse<LoginDTO>(new LoginDTO { UserId = user.Id, Username = user.Username, Email = user.Email, Language = user.Language, Roles = user.Roles }));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during login for email: {Email}", request.Email);
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new ApiResponse<string>("Error interno del servidor"));
            }
        }
    }
}
