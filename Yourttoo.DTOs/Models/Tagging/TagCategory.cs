using Yourttoo.DTOs.Common;

namespace Yourttoo.DTOs.Models.Tagging
{
    public class TagCategory : IEntity
    {
        public Guid Id { get; set; }
        public string Code { get; set; } = string.Empty;
        public MultiLanguageText Name { get; set; } = new();
        public MultiLanguageText Description { get; set; } = new();
        public bool Filtering { get; set; }
        public string CreatedBy { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }

        // Navigation property
        public List<Tag>? Tags { get; set; }
    }
}
