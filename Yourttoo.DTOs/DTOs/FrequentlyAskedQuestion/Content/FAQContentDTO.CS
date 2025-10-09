using Yourttoo.DTOs.Common;

namespace Yourttoo.DTOs.DTOs.FrequentlyAskedQuestion.Content
{
    public class FAQContentDTO : IDataTransferObject
    {
        public Guid Id { get; set; } = Guid.Empty;
        public MultiLanguageText Title { get; set; } = new();
        public MultiLanguageText Content { get; set; } = new();
        public string Status { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public Guid FAQSectionId { get; set; } = Guid.Empty;
    }
}