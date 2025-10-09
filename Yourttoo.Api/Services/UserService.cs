using Microsoft.EntityFrameworkCore;
using Yourttoo.Api.DataAccess;
using Yourttoo.DTOs.DTOs.Authentication;
using Yourttoo.DTOs.Models.Authentication;
using System.Security.Cryptography;
using System.Text;

namespace Yourttoo.Api.Services
{
    public class UserService : IUserService
    {
        private readonly DatabaseContext _context;
        private readonly ILogger<UserService> _logger;

        public UserService(DatabaseContext context, ILogger<UserService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<UserDTO?> GetUserByUsernameAsync(string username)
        {
            try
            {
                var user = await _context.Users
                    .Include(u => u.UserRoles)
                        .ThenInclude(ur => ur.Role)
                    .FirstOrDefaultAsync(u => u.Username == username && u.IsActive);

                if (user == null)
                    return null;

                return MapUserToDTO(user);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting user by username: {Username}", username);
                return null;
            }
        }

        public async Task<UserDTO?> GetUserByEmailAsync(string email)
        {
            try
            {
                var user = await _context.Users
                    .Include(u => u.UserRoles)
                        .ThenInclude(ur => ur.Role)
                    .FirstOrDefaultAsync(u => u.Email == email && u.IsActive);

                if (user == null)
                    return null;

                return MapUserToDTO(user);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting user by email: {Email}", email);
                return null;
            }
        }

        public async Task<UserDTO?> GetUserWithRolesAsync(Guid userId)
        {
            try
            {
                var user = await _context.Users
                    .Include(u => u.UserRoles)
                        .ThenInclude(ur => ur.Role)
                    .FirstOrDefaultAsync(u => u.Id == userId && u.IsActive);

                if (user == null)
                    return null;

                return MapUserToDTO(user);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting user with roles: {UserId}", userId);
                return null;
            }
        }

        public async Task<bool> ValidateUserCredentialsAsync(string username, string password)
        {
            try
            {
                var user = await _context.Users
                    .FirstOrDefaultAsync(u => u.Username == username && u.IsActive);

                if (user == null)
                    return false;

                // TODO: Implementar hash de contraseña real
                // Por ahora, validación básica para demo
                return user.PasswordHash == HashPassword(password);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error validating user credentials: {Username}", username);
                return false;
            }
        }

        public async Task<UserDTO?> CreateUserAsync(string username, string email, string password, string firstName, string lastName, string? avatar = null)
        {
            try
            {
                var user = new User
                {
                    Id = Guid.NewGuid(),
                    Username = username,
                    Email = email,
                    PasswordHash = HashPassword(password),
                    FirstName = firstName,
                    LastName = lastName,
                    Avatar = avatar,
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow
                };

                _context.Users.Add(user);
                await _context.SaveChangesAsync();

                _logger.LogInformation("User created: {Username}", username);

                return MapUserToDTO(user);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating user: {Username}", username);
                return null;
            }
        }

        public async Task<bool> AssignRoleToUserAsync(Guid userId, Guid roleId)
        {
            try
            {
                // Verificar si ya existe la asignación
                var existingUserRole = await _context.UserRoles
                    .FirstOrDefaultAsync(ur => ur.UserId == userId && ur.RoleId == roleId);

                if (existingUserRole != null)
                    return true; // Ya existe

                var userRole = new UserRole
                {
                    Id = Guid.NewGuid(),
                    UserId = userId,
                    RoleId = roleId,
                    AssignedAt = DateTime.UtcNow,
                    IsActive = true
                };

                _context.UserRoles.Add(userRole);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Role assigned to user: {UserId}, RoleId: {RoleId}", userId, roleId);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error assigning role to user: {UserId}, RoleId: {RoleId}", userId, roleId);
                return false;
            }
        }

        public async Task<List<RoleDTO>> GetUserRolesAsync(Guid userId)
        {
            try
            {
                var userRoles = await _context.UserRoles
                    .Include(ur => ur.Role)
                    .Where(ur => ur.UserId == userId && ur.IsActive && ur.Role.IsActive)
                    .Select(ur => new RoleDTO
                    {
                        Id = ur.Role.Id,
                        Name = ur.Role.Name,
                        Description = ur.Role.Description,
                        IsActive = ur.Role.IsActive
                    })
                    .ToListAsync();

                return userRoles;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting user roles: {UserId}", userId);
                return new List<RoleDTO>();
            }
        }


        private UserDTO MapUserToDTO(User user)
        {
            return new UserDTO
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Avatar = user.Avatar,
                IsActive = user.IsActive,
                CreatedAt = user.CreatedAt,
                Roles = user.UserRoles
                    .Where(ur => ur.IsActive && ur.Role.IsActive)
                    .Select(ur => new RoleDTO
                    {
                        Id = ur.Role.Id,
                        Name = ur.Role.Name,
                        Description = ur.Role.Description,
                        IsActive = ur.Role.IsActive
                    })
                    .ToList()
            };
        }

        private string HashPassword(string password)
        {
            // TODO: Implementar hash seguro (BCrypt, Argon2, etc.)
            // Por ahora, hash simple para demo
            using var sha256 = SHA256.Create();
            var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(hashedBytes);
        }
    }
}
