using Yourttoo.DTOs.Shared.Features;

namespace Yourttoo.DTOs.Requests.Account
{
    public class PatchAccountRequest : RequestPayloadBase
    {
        public string? Email { get; set; }
        public string? Status { get; set; }
        public bool? IsTwoFactorEnabled { get; set; }
        public string? TwoFactorMethod { get; set; }
        public string? AccountType { get; set; }
        public IList<string>? ApiKeys { get; set; }
        
    }
}
