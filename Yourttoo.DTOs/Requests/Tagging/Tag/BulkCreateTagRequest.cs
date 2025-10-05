using Yourttoo.DTOs.Shared.Features;

namespace Yourttoo.DTOs.Requests.Tagging.Tag
{
    public class BulkCreateTagRequest : RequestPayloadBase
    {
        public List<CreateTagRequest> Tags { get; set; } = new();

        public BulkCreateTagRequest() { }
    }
}
