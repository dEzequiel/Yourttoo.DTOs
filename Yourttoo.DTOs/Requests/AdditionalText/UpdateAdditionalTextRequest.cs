using Yourttoo.DTOs.Common;
using Yourttoo.DTOs.Shared.Features;

namespace Yourttoo.DTOs.Requests.AdditionalText
{
    public class UpdateAdditionalTextRequest : RequestPayloadBase
    {
        public Guid Id { get; set; } = Guid.Empty;
        public List<IdiomaticText> Title { get; set; } = new();
        public List<IdiomaticText>? Content { get; set; } = new();
        public string Status { get; set; } = string.Empty;

        public UpdateAdditionalTextRequest() { }
    }
}