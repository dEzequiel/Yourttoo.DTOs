using Yourttoo.DTOs.Shared.Features;

namespace Yourttoo.DTOs.Requests.Account
{
    public class UpdateAccountRequest : RequestPayloadBase
    {
        public string Email { get; set; } = string.Empty;
        public string Status { get; set; } = "active";
        public bool IsTwoFactorEnabled { get; set; } = false;
        public string? TwoFactorMethod { get; set; }
        public string AccountType { get; set; } = "agency";
        public IList<string> ApiKeys { get; set; } = new List<string>();
    }
}
