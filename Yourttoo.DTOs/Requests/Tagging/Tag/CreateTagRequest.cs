using Yourttoo.DTOs.Common;
using Yourttoo.DTOs.Shared.Features;

namespace Yourttoo.DTOs.Requests.Tagging.Tag
{
    public class CreateTagRequest : RequestPayloadBase
    {
        public string Code { get; set; } = string.Empty;
        public List<IdiomaticText> Name { get; set; } = new();
        public List<IdiomaticText> Description { get; set; } = new();
        public List<IdiomaticText> Label { get; set; } = new();
        public string Slug { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public string Icon { get; set; } = string.Empty;
        public List<Guid> Categories { get; set; } = new();

        public CreateTagRequest() { }
    }
}
