using Yourttoo.DTOs.DTOs.Users;
using Yourttoo.DTOs.Requests.User;
namespace Yourttoo.Api.Services
{
    public interface IUserService
    {
        Task<UserDTO?> GetUserByIdAsync(Guid id);
        Task<UserDTO?> GetUserByEmailAsync(string email);
        Task<UserDTO?> CreateUserAsync(string username, string firstName, string lastName, string? language, string? timeZone, string? accountId, string email, string password, string status, IList<string> roles, string createdBy);
    }
}
