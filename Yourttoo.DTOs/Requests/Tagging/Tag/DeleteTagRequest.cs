using Yourttoo.DTOs.Shared.Features;

namespace Yourttoo.DTOs.Requests.Tagging.Tag
{
    public class DeleteTagRequest : RequestPayloadBase
    {
        public Guid Id { get; set; } = Guid.Empty;

        public DeleteTagRequest() { }
    }
}
