using Yourttoo.DTOs.Common;

namespace Yourttoo.DTOs.DTOs.FrequentlyAskedQuestion.Section
{
    public class FAQSectionDTO : IDataTransferObject
    {
        public Guid Id { get; set; } = Guid.Empty;
        public MultiLanguageText Title { get; set; } = new();
        public string? Category { get; set; } = null;
        public string? Type { get; set; } = null;
        public string CreatedBy { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public List<Guid> Contents { get; set; } = new();
    }
}