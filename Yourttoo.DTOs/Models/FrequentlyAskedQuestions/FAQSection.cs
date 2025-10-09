using Yourttoo.DTOs.Common;
using Yourttoo.DTOs.Models.FrequentlyAskedQuestions;

namespace Yourttoo.DTOs.Models.FrequentlyAskedQuestions
{
    public class FAQSection : IEntity
    {
        public Guid Id { get; set; }
        public List<MultiLanguageText> Title { get; set; } = new();
        public string? Category { get; set; } = null;
        public string? Type { get; set; } = null;
        public string CreatedBy { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }

        // Navigation property
        public List<FAQContent> Contents { get; set; } = [];
    }
}