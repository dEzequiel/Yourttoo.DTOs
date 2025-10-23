using Yourttoo.DTOs.Common;

namespace Yourttoo.DTOs.Models
{
    public class AdditionalText : IEntity
    {
        public Guid Id { get; set; }
        public MultiLanguageText Title { get; set; } = new();
        public MultiLanguageText? Content { get; set; } = new();
        public string Status { get; set; } = string.Empty;
        public string CreatedBy { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
