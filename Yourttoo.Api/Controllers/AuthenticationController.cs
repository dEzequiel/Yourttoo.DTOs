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
                }

                var login = new LoginDTO { UserId = user.Id, Username = user.Username, Email = user.Email, Language = user.Language, Roles = user.Roles };
                _logger.LogInformation("Login successful for email: {Email}", request.Email);
                return Ok(new ApiResponse<LoginDTO>(login));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during login for email: {Email}", request.Email);
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new ApiResponse<string>("Error interno del servidor"));
            }
        }

        private string GenerateToken(string sessionId)
        {
            // Token simple basado en la sesión
            // En producción, usar JWT o similar
            var payload = $"{sessionId}:{DateTime.UtcNow:O}";
            var bytes = System.Text.Encoding.UTF8.GetBytes(payload);
            return Convert.ToBase64String(bytes);
        }

        [HttpPost("logout")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Logout([FromBody] LogoutRequest request)
        {
            _logger.LogInformation("AuthController --> Logout --> Start for session: {SessionId}", request.SessionId);

            try
            {
                var success = await _sessionService.LogoutAsync(request.SessionId);

                if (!success)
                {
                    _logger.LogWarning("Logout failed for session: {SessionId}", request.SessionId);
                    return BadRequest(new ApiResponse<string>("Sesión no encontrada o ya expirada"));
                }

                _logger.LogInformation("Logout successful for session: {SessionId}", request.SessionId);
                return Ok(new ApiResponse<string>("Sesión cerrada correctamente", true));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during logout for session: {SessionId}", request.SessionId);
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new ApiResponse<string>("Error interno del servidor"));
            }
        }

        [HttpGet("validate/{sessionId}")]
        [ProducesResponseType(typeof(SessionWithUserDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ValidateSession(string sessionId)
        {
            _logger.LogInformation("AuthController --> ValidateSession --> Start for session: {SessionId}", sessionId);

            try
            {
                var sessionWithUser = await _sessionService.GetSessionWithUserAsync(sessionId);

                if (sessionWithUser == null)
                {
                    _logger.LogWarning("Session validation failed for: {SessionId}", sessionId);
                    return Unauthorized(new ApiResponse<string>("Sesión inválida o expirada"));
                }

                _logger.LogInformation("Session validation successful for: {SessionId}, User: {Email}", 
                    sessionId, sessionWithUser.User.Email);
                return Ok(new ApiResponse<SessionWithUserDTO>(sessionWithUser));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error validating session: {SessionId}", sessionId);
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new ApiResponse<string>("Error interno del servidor"));
            }
        }

        [HttpGet("get-user-by-session-id/{sessionId}")]
        [ProducesResponseType(typeof(UserDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetUserBySessionId(string sessionId) {
            _logger.LogInformation("AuthController --> GetUserBySessionId --> Start for session: {SessionId}", sessionId);
            try {
                var user = await _sessionService.GetUserBySessionIdAsync(sessionId);
                if (user == null) {
                    return NotFound(new ApiResponse<string>("Usuario no encontrado"));
                }
                return Ok(new ApiResponse<UserDTO>(user));
            } catch (Exception ex) {
                _logger.LogError(ex, "Error getting user by session id: {SessionId}", sessionId);
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse<string>("Error interno del servidor"));
            }
        }

        [HttpPost("cleanup")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CleanupExpiredSessions()
        {
            _logger.LogInformation("AuthController --> CleanupExpiredSessions --> Start");

            try
            {
                await _sessionService.CleanupExpiredSessionsAsync();
                _logger.LogInformation("Cleanup completed successfully");
                return Ok(new ApiResponse<string>("Limpieza de sesiones completada"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during cleanup");
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new ApiResponse<string>("Error interno del servidor"));
            }
        }

    }
}
