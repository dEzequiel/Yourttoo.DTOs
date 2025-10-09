using Yourttoo.DTOs.Common;

namespace Yourttoo.DTOs.DTOs.Tagging.TagCategory
{
    public class TagCategoryDTO : IDataTransferObject
    {
        public Guid Id { get; set; } = Guid.Empty;
        public string Code { get; set; } = string.Empty;
        public MultiLanguageText Name { get; set; } = new();
        public MultiLanguageText Description { get; set; } = new();
        public bool Filtering { get; set; }
        public List<Guid> Tags { get; set; } = new();
    }
}
