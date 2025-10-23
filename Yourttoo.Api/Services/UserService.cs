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


        public async Task<UserDTO?> GetUserByIdAsync(string userId)
        {
            try
            {
                var user = await _context.Users
                    .Include(u => u.UserRoles)
                    .ThenInclude(ur => ur.Role)
                    .AsNoTracking()
                    .FirstOrDefaultAsync(u => u.Id == Guid.Parse(userId) && u.IsActive);
                if (user == null)
                {
                    _logger.LogWarning("User not found: {UserId}", userId);
                    return null;
                }

                return MapUserToDTO(user);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting user by id: {UserId}", userId);
                return null;
            }
        }

        public async Task<UserDTO?> GetUserByEmailAsync(string email)
        {
            try
            {
                var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email && u.IsActive);
                if (user == null)
                {
                    _logger.LogWarning("User not found: {Email}", email);
                    return null;
                }

                return MapUserToDTO(user);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting user by email: {Email}", email);
                return null;
            }
        }

        public async Task<UserDTO?> CreateUserAsync(string username, string email, string password, string firstName,
            string lastName, string? avatar = null)
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
            using var sha256 = SHA256.Create();
            var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(hashedBytes);
        }
    }
}