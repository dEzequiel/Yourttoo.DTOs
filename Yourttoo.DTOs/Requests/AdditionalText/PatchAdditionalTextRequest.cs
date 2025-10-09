using Yourttoo.DTOs.Common;
using Yourttoo.DTOs.Shared.Features;

namespace Yourttoo.DTOs.Requests.AdditionalText

{
    public class PatchAdditionalTextRequest : RequestPayloadBase
    {
        public Guid Id { get; set; } = Guid.Empty;
        public List<MultiLanguageText> Title { get; set; } = new();
        public List<MultiLanguageText>? Content { get; set; } = new();
        public string Status { get; set; } = string.Empty;

        public PatchAdditionalTextRequest() { }
    }
}