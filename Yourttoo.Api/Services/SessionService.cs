using Microsoft.EntityFrameworkCore;
using Yourttoo.Api.DataAccess;
using Yourttoo.DTOs.DTOs.Authentication;
using Yourttoo.DTOs.Models.Authentication;
using Yourttoo.DTOs.Requests.Authentication;
using System.Security.Cryptography;
using System.Text;

namespace Yourttoo.Api.Services
{
    public class SessionService : ISessionService
    {
        private readonly DatabaseContext _context;
        private readonly ILogger<SessionService> _logger;
        private readonly IUserService _userService;

        public SessionService(DatabaseContext context, ILogger<SessionService> logger, IUserService userService)
        {
            _context = context;
            _logger = logger;
            _userService = userService;
        }

        public async Task<SessionDTO?> CreateSessionAsync(CreateSessionRequest request, string? ipAddress = null, string? userAgent = null)
        {
            try
            {
                // Buscar usuario por email
                var user = await _userService.GetUserByEmailAsync(request.Email);
                if (user == null)
                {
                    _logger.LogWarning("User not found for email: {Email}", request.Email);
                    return null;
                }

                // Crear nueva sesión
                var sessionId = GenerateSessionId();
                var expiresAt = request.RememberMe 
                    ? DateTime.UtcNow.AddDays(30) // 30 días si "Remember Me"
                    : DateTime.UtcNow.AddHours(24); // 24 horas por defecto

                var session = new Session
                {
                    Id = Guid.NewGuid(),
                    SessionId = sessionId,
                    UserId = user.Id,
                    CreatedAt = DateTime.UtcNow,
                    ExpiresAt = expiresAt,
                    IsActive = true,
                    IpAddress = ipAddress,
                    UserAgent = userAgent
                };

                _context.Sessions.Add(session);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Session created for user: {Email}, SessionId: {SessionId}", request.Email, sessionId);

                return new SessionDTO
                {
                    Id = session.Id,
                    SessionId = sessionId,
                    UserId = user.Id.ToString(),
                    ExpiresAt = expiresAt,
                    IsActive = true
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating session for email: {Email}", request.Email);
                return null;
            }
        }

        public async Task<LoginResponse?> LoginAsync(LoginRequest request, string? ipAddress = null, string? userAgent = null)
        {
            try
            {
                // Validar email
                if (string.IsNullOrWhiteSpace(request.Email))
                {
                    return null;
                }

                // Obtener información del usuario con roles
                var user = await _userService.GetUserByEmailAsync(request.Email);
                if (user == null)
                {
                    _logger.LogWarning("User not found: {Email}", request.Email);
                    return null;
                }

                // Crear nueva sesión
                var sessionId = GenerateSessionId();
                var expiresAt = request.RememberMe 
                    ? DateTime.UtcNow.AddDays(30) // 30 días si "Remember Me"
                    : DateTime.UtcNow.AddHours(24); // 24 horas por defecto

                var session = new Session
                {
                    Id = Guid.NewGuid(),
                    SessionId = sessionId,
                    UserId = user.Id,
                    CreatedAt = DateTime.UtcNow,
                    ExpiresAt = expiresAt,
                    IsActive = true,
                    IpAddress = ipAddress,
                    UserAgent = userAgent
                };

                _context.Sessions.Add(session);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Session created for user: {Email}, SessionId: {SessionId}", request.Email, sessionId);

                return new LoginResponse
                {
                    SessionId = sessionId,
                    UserId = user.Id.ToString(),
                    ExpiresAt = expiresAt,
                    Token = GenerateToken(sessionId),
                    User = user
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during login for email: {Email}", request.Email);
                return null;
            }
        }

        public async Task<bool> LogoutAsync(string sessionId)
        {
            try
            {
                var session = await _context.Sessions
                    .FirstOrDefaultAsync(s => s.SessionId == sessionId && s.IsActive);

                if (session == null)
                    return false;

                _context.Sessions.Remove(session);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Session deleted: {SessionId}", sessionId);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error logging out session: {SessionId}", sessionId);
                return false;
            }
        }

        public async Task<bool> IsSessionValidAsync(string sessionId)
        {
            try
            {
                var session = await _context.Sessions
                    .FirstOrDefaultAsync(s => s.SessionId == sessionId && s.IsActive && s.ExpiresAt > DateTime.UtcNow);

                return session != null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error validating session: {SessionId}", sessionId);
                return false;
            }
        }

        public async Task<SessionDTO?> GetSessionAsync(string sessionId)
        {
            try
            {
                var session = await _context.Sessions
                    .FirstOrDefaultAsync(s => s.SessionId == sessionId && s.IsActive && s.ExpiresAt > DateTime.UtcNow);

                if (session == null)
                    return null;

                return new SessionDTO
                {
                    Id = session.Id,
                    SessionId = session.SessionId,
                    UserId = session.UserId.ToString(),
                    ExpiresAt = session.ExpiresAt,
                    IsActive = session.IsActive
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting session: {SessionId}", sessionId);
                return null;
            }
        }

        public async Task<SessionWithUserDTO?> GetSessionWithUserAsync(string sessionId)
        {
            try
            {
                var session = await _context.Sessions
                    .Include(s => s.User)
                    .ThenInclude(u => u.UserRoles)
                    .ThenInclude(ur => ur.Role)
                    .FirstOrDefaultAsync(s => s.SessionId == sessionId && s.IsActive && s.ExpiresAt > DateTime.UtcNow);

                if (session == null)
                    return null;

                var user = await _userService.GetUserWithRolesAsync(session.UserId);
                if (user == null)
                    return null;

                return new SessionWithUserDTO
                {
                    Id = session.Id,
                    SessionId = session.SessionId,
                    UserId = session.UserId.ToString(),
                    ExpiresAt = session.ExpiresAt,
                    IsActive = session.IsActive,
                    User = user
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting session with user: {SessionId}", sessionId);
                return null;
            }
        }

        public async Task<SessionDTO?> GetSessionByUserIdAsync(string userId)
        {
            try
            {
                if (!Guid.TryParse(userId, out var userIdGuid))
                {
                    _logger.LogWarning("Invalid user ID format: {UserId}", userId);
                    return null;
                }

                var session = await _context.Sessions
                    .FirstOrDefaultAsync(s => s.UserId == userIdGuid && s.IsActive && s.ExpiresAt > DateTime.UtcNow);

                if (session == null)
                    return null;

                return new SessionDTO
                {
                    Id = session.Id,
                    SessionId = session.SessionId,
                    UserId = session.UserId.ToString(),
                    ExpiresAt = session.ExpiresAt,
                    IsActive = session.IsActive
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting session by user ID: {UserId}", userId);
                return null;
            }
        }

        public async Task CleanupExpiredSessionsAsync()
        {
            try
            {
                var expiredSessions = await _context.Sessions
                    .Where(s => s.ExpiresAt <= DateTime.UtcNow || !s.IsActive)
                    .ToListAsync();

                if (expiredSessions.Any())
                {
                    _context.Sessions.RemoveRange(expiredSessions);
                    await _context.SaveChangesAsync();
                    _logger.LogInformation("Cleaned up {Count} expired sessions", expiredSessions.Count);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error cleaning up expired sessions");
            }
        }

        private string GenerateSessionId()
        {
            using var rng = RandomNumberGenerator.Create();
            var bytes = new byte[32];
            rng.GetBytes(bytes);
            return Convert.ToBase64String(bytes).Replace("+", "-").Replace("/", "_").Replace("=", "");
        }

        private string GenerateToken(string sessionId)
        {
            // Token simple basado en la sesión
            // En producción, usar JWT o similar
            var payload = $"{sessionId}:{DateTime.UtcNow:O}";
            var bytes = Encoding.UTF8.GetBytes(payload);
            return Convert.ToBase64String(bytes);
        }
    }
}
