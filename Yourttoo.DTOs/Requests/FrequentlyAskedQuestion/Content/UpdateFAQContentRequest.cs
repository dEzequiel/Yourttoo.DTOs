using Yourttoo.DTOs.Common;
using Yourttoo.DTOs.Shared.Features;

namespace Yourttoo.DTOs.Requests.FrequentlyAskedQuestion.Content
{
    public class UpdateFAQContentRequest : RequestPayloadBase
    {
        public List<MultiLanguageText> Title { get; set; } = new();
        public List<MultiLanguageText> Content { get; set; } = new();
        public string Status { get; set; } = string.Empty;
        public string Slug { get; set; } = string.Empty;
        public Guid FAQSectionId { get; set; } = Guid.Empty;
        
        public UpdateFAQContentRequest() { }
    }
}
