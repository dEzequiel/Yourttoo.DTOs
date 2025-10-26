using Yourttoo.DTOs.Shared.Features;

namespace Yourttoo.DTOs.Requests.Account
{
    public class ChangeAccountPasswordRequest : RequestPayloadBase
    {
        public string Email { get; set; } = string.Empty;
        public string CurrentPassword { get; set; } = string.Empty;
        public string NewPassword { get; set; } = string.Empty;
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}
