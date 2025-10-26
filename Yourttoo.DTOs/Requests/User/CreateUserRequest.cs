using Yourttoo.DTOs.Shared.Features;
using Yourttoo.DTOs.Common.Constants;

namespace Yourttoo.DTOs.Requests.User
{
    public class CreateUserRequest : RequestPayloadBase
    {
        public string? Username { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string Language { get; set; } = "es";
        public string TimeZone { get; set; } = "Europe/Madrid";
        public string? Avatar { get; set; }
        public string? AccountId { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public string Status { get; set; } = UserStatus.ACTIVE; // Default to active
        public IList<string> Roles { get; set; } = new List<string>();
    }
}
