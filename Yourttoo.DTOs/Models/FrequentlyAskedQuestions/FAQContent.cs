using Yourttoo.DTOs.Common;

namespace Yourttoo.DTOs.Models.FrequentlyAskedQuestions
{
    public class FAQContent : IEntity
    {
        public Guid Id { get; set; }
        public List<MultiLanguageText> Title { get; set; } = new();
        public List<MultiLanguageText> Content { get; set; } = new();

        public string Slug { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public Guid FAQSectionId { get; set; } = Guid.Empty;
        public string CreatedBy { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }

        // Navigation property
        public FAQSection FAQSection { get; set; } = new();
    }
}