using Yourttoo.DTOs.Shared.Features;

namespace Yourttoo.DTOs.Requests.Account
{
    public class LoginAccountRequest : RequestPayloadBase
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public bool RememberMe { get; set; } = false;
        public string? TwoFactorCode { get; set; }
    }
}
