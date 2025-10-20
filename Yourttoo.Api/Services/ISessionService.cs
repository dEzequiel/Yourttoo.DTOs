using Yourttoo.DTOs.DTOs.Authentication;
using Yourttoo.DTOs.Requests.Authentication;

namespace Yourttoo.Api.Services
{
    public interface ISessionService
    {
        Task<SessionDTO?> CreateSessionAsync(CreateSessionRequest request, string? ipAddress = null, string? userAgent = null);
        Task<LoginResponse?> LoginAsync(LoginRequest request, string? ipAddress = null, string? userAgent = null);
        Task<bool> LogoutAsync(string sessionId);
        Task<bool> IsSessionValidAsync(string sessionId);
        Task<SessionDTO?> GetSessionAsync(string sessionId);
        Task<SessionWithUserDTO?> GetSessionWithUserAsync(string sessionId);
        Task<SessionDTO?> GetSessionByUserIdAsync(string userId);
        Task CleanupExpiredSessionsAsync();
        Task<UserDTO?> GetUserBySessionIdAsync(string sessionId);
    }
}
