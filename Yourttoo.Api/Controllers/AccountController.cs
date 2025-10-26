using Microsoft.AspNetCore.Mvc;
using Yourttoo.Api.Services;
using Yourttoo.DTOs.DTOs.Users;
using Yourttoo.DTOs.Requests.Account;
using Yourttoo.DTOs.Shared.API;
using Microsoft.AspNetCore.Authorization;
namespace Yourttoo.Api.Controllers
{
    [Route("/store/account")]
    [ApiController]
    [AllowAnonymous]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;
        private readonly ILogger<AccountController> _logger;

        public AccountController(IAccountService accountService, ILogger<AccountController> logger)
        {
            _accountService = accountService;
            _logger = logger;
        }

        [HttpGet("get")]
        [ProducesResponseType(typeof(AccountDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAccountByEmail([FromQuery] string email)
        {
            _logger.LogInformation("AccountController --> GetAccountByEmail --> Start for email: {Email}", email);
            try
            {
                var account = await _accountService.GetAccountByEmailAsync(email);
                if (account == null)
                {
                    _logger.LogWarning("Account not found for email: {Email}", email);
                    return NotFound(new ApiResponse<string>("Error al obtener la cuenta"));
                }

                _logger.LogInformation("AccountController --> GetAccountByEmail --> End for email: {Email}", email);
                return Ok(new ApiResponse<AccountDTO>(account));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting account by email: {Email}", email);
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse<string>("Error interno del servidor"));
            }
        }

        [HttpPost("create")]
        [ProducesResponseType(typeof(AccountDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateAccount([FromBody] CreateAccountRequest request)
        {
            _logger.LogInformation("AccountController --> CreateAccount --> Start for email: {Email}", request.Email);
            try
            {
                var account = await _accountService.CreateAccountAsync(request.Email, request.Password, request.AccountType, request.CreatedBy ?? "System");
                if (account == null)
                {
                    _logger.LogWarning("Account not created for email: {Email}", request.Email);
                    return BadRequest(new ApiResponse<string>("Error al crear la cuenta"));
                }
                _logger.LogInformation("AccountController --> CreateAccount --> End for email: {Email}", request.Email);
                return CreatedAtAction(nameof(GetAccountByEmail), new { email = account.Email }, new ApiResponse<AccountDTO>(account));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating account for email: {Email}", request.Email);
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse<string>("Error interno del servidor"));
            }
        }
    }
}