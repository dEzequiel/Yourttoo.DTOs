using Yourttoo.DTOs.DTOs.Authentication;
using Yourttoo.DTOs.DTOs.Users;

namespace Yourttoo.Api.Services
{
    public interface IUserService
    {
        Task<UserDTO?> GetUserByEmailAsync(string email);
        Task<UserDTO?> GetUserByIdAsync(Guid id);
        Task<UserDTO?> CreateUserAsync(string username, string email, string password, string firstName, string lastName, string? avatar = null);
    }
}
