using Yourttoo.DTOs.Shared.Features;

namespace Yourttoo.DTOs.Requests.User
{
    public class LoginUserRequest : RequestPayloadBase
    {
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string? Email { get; set; }
        public bool RememberMe { get; set; } = false;
    }
}
