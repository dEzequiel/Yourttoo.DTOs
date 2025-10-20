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
        public AuthenticationController(IUserService userService, ILogger<AuthenticationController> logger)
        {
            _userService = userService;
            _logger = logger;
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

        [HttpPost("login")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(LoginResponse), StatusCodes.Status200OK)]
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
                return Ok(new ApiResponse<LoginResponse>(new LoginResponse { User = user }));
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
