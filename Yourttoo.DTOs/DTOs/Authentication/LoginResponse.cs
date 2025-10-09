using Yourttoo.DTOs.Common;

namespace Yourttoo.DTOs.DTOs.Authentication
{
    public class LoginResponse
    {
        public string SessionId { get; set; } = string.Empty;
        public string UserId { get; set; } = string.Empty;
        public DateTime ExpiresAt { get; set; }
        public string Token { get; set; } = string.Empty;
        public UserDTO User { get; set; } = new UserDTO();
    }
}
