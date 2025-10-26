using Yourttoo.DTOs.Shared.Features;

namespace Yourttoo.DTOs.Requests.User
{
    public class AssignRoleRequest : RequestPayloadBase
    {
        public Guid UserId { get; set; }
        public string Role { get; set; } = string.Empty;
    }
}
