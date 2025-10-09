using Yourttoo.DTOs.Common;

namespace Yourttoo.DTOs.DTOs.AdditionalText
{
    public class AdditionalTextDTO : IDataTransferObject
    {
        public Guid Id { get; set; } = Guid.Empty;
        public MultiLanguageText Title { get; set; } = new();
        public MultiLanguageText? Content { get; set; } = null;
        public string Status { get; set; } = string.Empty;
        public string CreatedBy { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
