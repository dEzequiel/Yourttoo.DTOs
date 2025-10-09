using Yourttoo.DTOs.Common;
using Yourttoo.DTOs.Shared.Features;

namespace Yourttoo.DTOs.Requests.FrequentlyAskedQuestion.Section
{
    public class CreateFAQSectionRequest : RequestPayloadBase
    {
        public List<MultiLanguageText> Title { get; set; } = new();
        public string? Category { get; set; } = null;
        public string? Type { get; set; } = null;
        
        public CreateFAQSectionRequest() { }
    }
}
