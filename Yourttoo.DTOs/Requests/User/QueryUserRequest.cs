using Yourttoo.DTOs.Shared.Features;

namespace Yourttoo.DTOs.Requests.User
{
    public class QueryUserRequest : RequestPayloadBase
    {
        public string? SearchTerm { get; set; }
        public string? Status { get; set; }
        public string? Username { get; set; }
        public string? Email { get; set; }
        public string? AccountId { get; set; }
        public string? Language { get; set; }
        public string? TimeZone { get; set; }
        public IList<string>? Roles { get; set; }
    }
}
