using Yourttoo.Api.DataAccess;
using Yourttoo.DTOs.DTOs.Users;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using Yourttoo.DTOs.Models.Users;
using Yourttoo.DTOs.Common.Constants;
namespace Yourttoo.Api.Services
{
    public class AccountService : IAccountService
    {
        private readonly DatabaseContext _context;
        private readonly ILogger<AccountService> _logger;
        private readonly IMapper _mapper;

        public AccountService(DatabaseContext context, ILogger<AccountService> logger, IMapper mapper)
        {
            _context = context;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<AccountDTO?> GetAccountByEmailAsync(string email)
        {
            try
            {
                var account = await _context.Accounts.FirstOrDefaultAsync(a => a.Email == email);
                if (account == null)
                {
                    _logger.LogWarning("Account not found for email: {Email}", email);
                    return null;
                }

                return _mapper.Map<Account, AccountDTO>(account);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting account by email: {Email}", email);
                return null;
            }
        }


        public async Task<AccountDTO?> CreateAccountAsync(string email, string password, string accountType, string createdBy)
        {
            try
            {
                var account = new Account
                {
                    Email = email,
                    PasswordHash = password,
                    Status = UserStatus.ACTIVE,
                    IsTwoFactorEnabled = false,
                    TwoFactorMethod = null,
                    AccountType = accountType,
                    ApiKeys = new List<string>()
                    {
                        "api_key_1"
                    },
                    CreatedBy = createdBy,
                    CreatedAt = DateTime.UtcNow
                };

                var accountAlreadyExists = await _context.Accounts.FirstOrDefaultAsync(a => a.Email == account.Email);
                if (accountAlreadyExists != null)
                {
                    _logger.LogWarning("Account already exists for email: {Email}", account.Email);
                    return null;
                }
                _context.Accounts.Add(account);
                await _context.SaveChangesAsync();
                return _mapper.Map<Account, AccountDTO>(account);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating account: {Email}", email);
                return null;
            }
        }
    }
}