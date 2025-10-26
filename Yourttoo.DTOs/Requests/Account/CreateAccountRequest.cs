using Yourttoo.DTOs.Shared.Features;
using Yourttoo.DTOs.Common.Constants;
namespace Yourttoo.DTOs.Requests.Account
{
    public class CreateAccountRequest : RequestPayloadBase
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Status { get; set; } = UserStatus.ACTIVE;
        public bool IsTwoFactorEnabled { get; set; } = false;
        public string? TwoFactorMethod { get; set; }
        public string AccountType { get; set; }
        public IList<string> ApiKeys { get; set; } = new List<string>();
    }
}
