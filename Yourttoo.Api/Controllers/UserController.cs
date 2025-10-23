using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Yourttoo.Api.Services;
using Yourttoo.DTOs.DTOs.Authentication;
using Yourttoo.DTOs.Common.Helpers;
using Yourttoo.DTOs.Shared.API;


namespace Yourttoo.Api.Controllers
{
    [Route("/store/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ILogger<UserController> _logger;

        public UserController(IUserService userService, ILogger<UserController> logger)
        {
            _userService = userService;
            _logger = logger;
        }

        [HttpGet("{userId}")]
        [ProducesResponseType(typeof(UserDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetUserById(string userId)
        {
            _logger.LogInformation("UserController --> GetUserById --> Start for user id: {UserId}", userId);
            try
            {
                var user = await _userService.GetUserByIdAsync(userId);
                if (user == null)
                {
                    _logger.LogWarning("User not found: {UserId}", userId);
                    return NotFound(new ApiResponse<string>("Usuario no encontrado"));
                }

                _logger.LogInformation("UserController --> GetUserById --> End for user id: {UserId}", userId);
                return Ok(new ApiResponse<UserDTO>(user));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting user by id: {UserId}", userId);
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new ApiResponse<string>("Error interno del servidor"));
            }
        }
    }
}
using Microsoft.AspNetCore.Mvc;
using Yourttoo.Api.Services;
using Yourttoo.DTOs.DTOs.Users;
using Yourttoo.DTOs.Requests.User;
using Yourttoo.DTOs.Shared.API;
using Microsoft.AspNetCore.Authorization;

namespace Yourttoo.Api.Controllers
{
    [Route("/store/user")]
    [ApiController]
    [AllowAnonymous]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ILogger<UserController> _logger;

        public UserController(IUserService userService, ILogger<UserController> logger)
        {
            _userService = userService;
            _logger = logger;
        }

        [HttpGet("get")]
        [ProducesResponseType(typeof(UserDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetUserByEmail([FromQuery] string email)
        {
            _logger.LogInformation("UserController --> GetUserByEmail --> Start for email: {Email}", email);
            try
            {
                var user = await _userService.GetUserByEmailAsync(email);
                if (user == null)
                {
                    _logger.LogWarning("User not found for email: {Email}", email);
                    return NotFound(new ApiResponse<string>("Error al obtener el usuario"));
                }

                _logger.LogInformation("UserController --> GetUserByEmail --> End for email: {Email}", email);
                return Ok(new ApiResponse<UserDTO>(user));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting user by email: {Email}", email);
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new ApiResponse<string>("Error interno del servidor"));
            }
        }


        [HttpGet("{id}")]
        [ProducesResponseType(typeof(UserDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetUserById(Guid id)
        {
            _logger.LogInformation("UserController --> GetUserById --> Start for id: {Id}", id);
            try
            {
                var user = await _userService.GetUserByIdAsync(id);
                if (user == null)
                {
                    _logger.LogWarning("User not found for id: {Id}", id);
                    return NotFound(new ApiResponse<string>("Error al obtener el usuario"));
                }

                _logger.LogInformation("UserController --> GetUserById --> End for id: {Id}", id);
                return Ok(new ApiResponse<UserDTO>(user));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting user by id: {Id}", id);
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new ApiResponse<string>("Error interno del servidor"));
            }
        }

        [HttpPost("create")]
        [ProducesResponseType(typeof(UserDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserRequest request)
        {
            _logger.LogInformation("UserController --> CreateUser --> Start for email: {Email}", request.Email);
            try
            {
                var user = await _userService.CreateUserAsync(request.Username, request.FirstName, request.LastName,
                    request.Language, request.TimeZone, request.AccountId, request.Email, request.Password,
                    request.Status, request.Roles, request.CreatedBy ?? "System");
                if (user == null)
                {
                    _logger.LogWarning("User not created for email: {Email}", request.Email);
                    return BadRequest(new ApiResponse<string>("Error al crear el usuario"));
                }

                _logger.LogInformation("UserController --> CreateUser --> End for email: {Email}", request.Email);
                return CreatedAtAction(nameof(GetUserByEmail), new { email = user.Email },
                    new ApiResponse<UserDTO>(user));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating user for email: {Email}", request.Email);
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new ApiResponse<string>("Error interno del servidor"));
            }
        }
    }
}