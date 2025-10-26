using Yourttoo.DTOs.DTOs.Users;
using Yourttoo.DTOs.Requests.Account;
namespace Yourttoo.Api.Services
{
    public interface IAccountService
    {
        Task<AccountDTO?> GetAccountByEmailAsync(string email);
        Task<AccountDTO?> CreateAccountAsync(string email, string password, string accountType, string createdBy);
    }
}