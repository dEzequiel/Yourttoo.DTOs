using Yourttoo.DTOs.Common;

namespace Yourttoo.DTOs.DTOs.FrequentlyAskedQuestion.Section
{
    public class FAQSectionDetailDTO : IDataTransferObject
    {
        public Guid Id { get; set; } = Guid.Empty;
        public MultiLanguageText Title { get; set; } = new();
        public string? Category { get; set; } = null;
        public string? Type { get; set; } = null;
        public List<Guid> Contents { get; set; } = new();
    }
}
