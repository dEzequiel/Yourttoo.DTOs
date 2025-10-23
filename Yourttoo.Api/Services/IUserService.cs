using Yourttoo.DTOs.DTOs.Authentication;

namespace Yourttoo.Api.Services
{
    public interface IUserService
    {
        Task<UserDTO?> GetUserByEmailAsync(string email);
        Task<UserDTO?> GetUserByIdAsync(string userId);
        Task<UserDTO?> CreateUserAsync(string username, string email, string password, string firstName, string lastName, string? avatar = null);
    }
}
