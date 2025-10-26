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
        }


        public async Task<UserDTO?> GetUserByIdAsync(Guid id)
        {
            try
            {
                var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
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

        public async Task<UserDTO?> GetUserByEmailAsync(string email)
        {
            try
            {
                var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
                if (user == null)
                {
                    _logger.LogWarning("User not found: {Email}", email);
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

        public async Task<UserDTO?> CreateUserAsync(string username, string email, string password, string firstName,
            string lastName, string? avatar = null)
        {
                var user = new User
                {
                    Username = username,
                    FirstName = firstName,
                    LastName = lastName,
                    Email = email,
                    PasswordHash = password,
                };

                _context.Users.Add(user);
                await _context.SaveChangesAsync();
                return _mapper.Map<User, UserDTO>(user);
        }

    }
}