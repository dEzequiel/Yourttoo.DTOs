using Yourttoo.DTOs.Shared.Features;

namespace Yourttoo.DTOs.Requests.Account
{
    public class QueryAccountRequest : RequestPayloadBase
    {
        public string? SearchTerm { get; set; }
        public string? Status { get; set; }
        public string? Email { get; set; }
        public string? AccountType { get; set; }
        public bool? IsTwoFactorEnabled { get; set; }
        public string? TwoFactorMethod { get; set; }
        public DateTime? LastLoginFrom { get; set; }
        public DateTime? LastLoginTo { get; set; }
        public int? MinFailedLoginAttempts { get; set; }
        public int? MaxFailedLoginAttempts { get; set; }
    }
}
