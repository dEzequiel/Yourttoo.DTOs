using Yourttoo.DTOs.Shared.Features;

namespace Yourttoo.DTOs.Requests.User
{
    public class ChangePasswordRequest : RequestPayloadBase
    {
        public Guid UserId { get; set; }
        public string CurrentPassword { get; set; } = string.Empty;
        public string NewPassword { get; set; } = string.Empty;
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}
