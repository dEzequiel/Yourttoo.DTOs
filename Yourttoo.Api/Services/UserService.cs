using Yourttoo.Api.DataAccess;
using Yourttoo.DTOs.DTOs.Users;
using Microsoft.EntityFrameworkCore;
using Yourttoo.DTOs.Models.Users;
using AutoMapper;
using Yourttoo.DTOs.Requests.User;
using Yourttoo.DTOs.Common.Constants;

namespace Yourttoo.Api.Services
{
    public class UserService : IUserService
    {
        private readonly DatabaseContext _context;
        private readonly ILogger<UserService> _logger;
        private readonly IMapper _mapper;

        public UserService(DatabaseContext context, ILogger<UserService> logger, IMapper mapper)
        {
            _context = context;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<UserDTO?> GetUserByEmailAsync(string email)
        {
            try
            {
                var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
                if (user == null)
                {
                    _logger.LogWarning("User not found for email: {Email}", email);
                    return null;
                }

                return _mapper.Map<User, UserDTO>(user);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting user by email: {Email}", email);
                return null;
            }
        }

        public async Task<UserDTO?> GetUserByIdAsync(Guid id)
        {
            try
            {
                var user = await _context.Users.FindAsync(id);
                if (user == null)
                {
                    _logger.LogWarning("User not found for id: {Id}", id);
                    return null;
                }

                return _mapper.Map<User, UserDTO>(user);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting user by id: {Id}", id);
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

        public async Task<UserDTO?> GetUserByIdAsync(string userId)
        {
            try {
                var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == Guid.Parse(userId) && u.IsActive);
                if (user == null) {
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

        public async Task<UserDTO?> CreateUserAsync(string username, string email, string password, string firstName, string lastName, string? avatar = null)
        {
            try
            {
                var user = new User
                {
                    Username = username,
                    FirstName = firstName,
                    LastName = lastName,
                    Language = language ?? Languages.Default,
                    TimeZone = timeZone ?? TimeZones.Europe.MADRID,
                    AccountId = accountId,
                    Email = email,
                    PasswordHash = password,
                    Status = status,
                    Roles = roles,
                    CreatedBy = createdBy,
                    CreatedAt = DateTime.UtcNow
                };

                var userAlreadyExists = await _context.Users.FirstOrDefaultAsync(u => u.Email == user.Email);
                if (userAlreadyExists != null)
                {
                    _logger.LogWarning("User already exists for email: {Email}", user.Email);
                    return null;
                }

                _context.Users.Add(user);
                await _context.SaveChangesAsync();
                return _mapper.Map<User, UserDTO>(user);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating user: {Email}", email);
                return null;
            }
        }
    }
}