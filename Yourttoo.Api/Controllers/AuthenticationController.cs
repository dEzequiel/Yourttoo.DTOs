using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Yourttoo.Api.Services;
using Yourttoo.DTOs.DTOs.Authentication;
using Yourttoo.DTOs.Models.Authentication;
using Yourttoo.DTOs.Requests.Authentication;
using Yourttoo.DTOs.Shared.API;

namespace Yourttoo.Api.Controllers
{
    [Route("/store/authentication")]
    [ApiController]
    [AllowAnonymous]
    public class AuthenticationController : ControllerBase
    {
        private readonly ISessionService _sessionService;
        private readonly IUserService _userService;
        private readonly ILogger<AuthenticationController> _logger;

        public AuthenticationController(ISessionService sessionService, IUserService userService, ILogger<AuthenticationController> logger)
        {
            _sessionService = sessionService;
            _userService = userService;
            _logger = logger;
        }

        [HttpGet("test")]
        [AllowAnonymous]
        public IActionResult Test()
        {
            return Ok(new { message = "Test endpoint working", timestamp = DateTime.UtcNow });
        }

        [HttpPost("register")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(UserDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            _logger.LogInformation("AuthenticationController --> Register --> Start for email: {Email}", request.Email);

            try
            {
                // Validar datos requeridos
                if (string.IsNullOrWhiteSpace(request.Username) || 
                    string.IsNullOrWhiteSpace(request.Email) || 
                    string.IsNullOrWhiteSpace(request.Password) ||
                    string.IsNullOrWhiteSpace(request.FirstName) || 
                    string.IsNullOrWhiteSpace(request.LastName))
                {
                    return BadRequest(new ApiResponse<string>("Todos los campos son requeridos"));
                }

                // Verificar si el usuario ya existe
                var existingUserByEmail = await _userService.GetUserByEmailAsync(request.Email);
                if (existingUserByEmail != null)
                {
                    return BadRequest(new ApiResponse<string>("El email ya está registrado"));
                }

                var existingUserByUsername = await _userService.GetUserByUsernameAsync(request.Username);
                if (existingUserByUsername != null)
                {
                    return BadRequest(new ApiResponse<string>("El nombre de usuario ya está en uso"));
                }

                // Crear usuario
                var user = await _userService.CreateUserAsync(
                    request.Username, 
                    request.Email, 
                    request.Password, 
                    request.FirstName, 
                    request.LastName, 
                    request.Avatar);

                if (user == null)
                {
                    _logger.LogWarning("User creation failed for email: {Email}", request.Email);
                    return BadRequest(new ApiResponse<string>("Error al crear el usuario"));
                }

                _logger.LogInformation("User registered successfully: {Email}", request.Email);
                return Ok(new ApiResponse<UserDTO>(user));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during registration for email: {Email}", request.Email);
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new ApiResponse<string>("Error interno del servidor"));
            }
        }

        [HttpPost("initialize-data")]
        [AllowAnonymous]
        public async Task<IActionResult> InitializeData()
        {
            try
            {

                // Crear usuario admin
                var adminUser = await _userService.CreateUserAsync("admin", "admin@admin.com", "password", "Admin", "User");
                if (adminUser != null)
                {
                    await _userService.AssignRoleToUserAsync(adminUser.Id, Guid.Parse("550e8400-e29b-41d4-a716-446655440001"));
                    await _userService.AssignRoleToUserAsync(adminUser.Id, Guid.Parse("550e8400-e29b-41d4-a716-446655440002"));
                }

                return Ok(new { message = "Datos inicializados correctamente", adminUserId = adminUser?.Id });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error initializing data");
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Error al inicializar datos" });
            }
        }

        [HttpPost("create-session")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(SessionDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateSession([FromBody] CreateSessionRequest request)
        {
            _logger.LogInformation("AuthenticationController --> CreateSession --> Start for email: {Email}", request.Email);

            try
            {
                var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString();
                var userAgent = HttpContext.Request.Headers["User-Agent"].FirstOrDefault();

                var session = await _sessionService.CreateSessionAsync(request, ipAddress, userAgent);

                if (session == null)
                {
                    _logger.LogWarning("Session creation failed for email: {Email}", request.Email);
                    return BadRequest(new ApiResponse<string>("Error al crear la sesión. Usuario no encontrado."));
                }

                _logger.LogInformation("Session created successfully for email: {Email}, SessionId: {SessionId}", 
                    request.Email, session.SessionId);

                return Ok(new ApiResponse<SessionDTO>(session));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating session for email: {Email}", request.Email);
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new ApiResponse<string>("Error interno del servidor"));
            }
        }

        [HttpPost("login")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(LoginResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            _logger.LogInformation("AuthenticationController --> Login --> Start for email: {Email}", request.Email);

            try
            {
                var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString();
                var userAgent = HttpContext.Request.Headers["User-Agent"].FirstOrDefault();

                var loginResponse = await _sessionService.LoginAsync(request, ipAddress, userAgent);

                if (loginResponse == null)
                {
                    _logger.LogWarning("Login failed for email: {Email}", request.Email);
                    return Unauthorized(new ApiResponse<string>("Usuario no encontrado"));
                }

                _logger.LogInformation("Login successful for email: {Email}, SessionId: {SessionId}", 
                    request.Email, loginResponse.SessionId);

                return Ok(new ApiResponse<LoginResponse>(loginResponse));
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
