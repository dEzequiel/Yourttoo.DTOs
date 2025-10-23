using Yourttoo.DTOs.Common;

namespace Yourttoo.DTOs.DTOs.FrequentlyAskedQuestion.Content
{
    public class FAQContentDetailDTO : IDataTransferObject
    {
        public Guid Id { get; set; } = Guid.Empty;
        public MultiLanguageText Title { get; set; } = new();
        public MultiLanguageText Content { get; set; } = new();
        public string Status { get; set; } = string.Empty;
        public string Slug { get; set; } = string.Empty;
        public Guid FAQSectionId { get; set; } = Guid.Empty;
    }
}