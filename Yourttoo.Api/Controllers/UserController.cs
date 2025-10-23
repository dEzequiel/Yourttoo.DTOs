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