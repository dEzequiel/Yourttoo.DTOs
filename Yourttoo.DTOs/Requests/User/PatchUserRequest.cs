using Yourttoo.DTOs.Shared.Features;

namespace Yourttoo.DTOs.Requests.User
{
    public class PatchUserRequest : RequestPayloadBase
    {
        public Guid Id { get; set; }
        public string? Username { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Language { get; set; }
        public string? TimeZone { get; set; }
        public string? AccountId { get; set; }
        public string? Avatar { get; set; }
        public string? Email { get; set; }
        public string? Status { get; set; }
        public IList<string>? Roles { get; set; }
    }
}
