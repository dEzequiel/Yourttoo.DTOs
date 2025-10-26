using Yourttoo.DTOs.Shared.Features;

namespace Yourttoo.DTOs.Requests.User
{
    public class DeleteUserRequest : RequestPayloadBase
    {
        public Guid Id { get; set; }
    }
}
