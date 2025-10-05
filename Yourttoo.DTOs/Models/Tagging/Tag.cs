
using Yourttoo.DTOs.Common;

namespace Yourttoo.DTOs.Models.Tagging
{
    public class Tag : IEntity
    {

        public Guid Id { get; set; }
        public string Code { get; set; } = string.Empty;
        public List<IdiomaticText> Name { get; set; } = new();
        public List<IdiomaticText> Description { get; set; } = new();
        public List<IdiomaticText> Label { get; set; } = new();
        public string Slug { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public string Icon { get; set; } = string.Empty;
        public string CreatedBy { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }

        // Navigation property
        public List<TagCategory> Categories { get; set; } = [];
    }
}
