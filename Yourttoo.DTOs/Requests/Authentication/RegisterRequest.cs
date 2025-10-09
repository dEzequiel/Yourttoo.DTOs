using Yourttoo.DTOs.Shared.Features;

namespace Yourttoo.DTOs.Requests.Authentication
{
    public class RegisterRequest
    {
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string? Avatar { get; set; }
        public DateTime CreatedAt { get; set; }
        
        public RegisterRequest() { }
    }
}
