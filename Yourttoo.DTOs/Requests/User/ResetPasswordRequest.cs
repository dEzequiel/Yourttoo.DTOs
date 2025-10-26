using Yourttoo.DTOs.Shared.Features;

namespace Yourttoo.DTOs.Requests.User
{
    public class ResetPasswordRequest : RequestPayloadBase
    {
        public string Email { get; set; } = string.Empty;
        public string? ResetToken { get; set; }
        public string? NewPassword { get; set; }
    }
}
