using Yourttoo.DTOs.DTOs.Authentication;
using Yourttoo.DTOs.Requests.Authentication;

namespace Yourttoo.Api.Services
{
    public interface IUserService
    {
        Task<UserDTO?> GetUserByUsernameAsync(string username);
        Task<UserDTO?> GetUserByEmailAsync(string email);
        Task<UserDTO?> GetUserWithRolesAsync(Guid userId);
        Task<bool> ValidateUserCredentialsAsync(string username, string password);
        Task<UserDTO?> CreateUserAsync(string username, string email, string password, string firstName, string lastName, string? avatar = null);
        Task<bool> AssignRoleToUserAsync(Guid userId, Guid roleId);
        Task<List<RoleDTO>> GetUserRolesAsync(Guid userId);
    }
}
