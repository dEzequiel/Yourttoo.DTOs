using Yourttoo.DTOs.Common;

namespace Yourttoo.DTOs.DTOs.Tagging.TagCategory
{
    public class TagCategoryDTO : IDataTransferObject
    {
        public Guid Id { get; set; } = Guid.Empty;
        public string Code { get; set; } = string.Empty;
        public IdiomaticText Name { get; set; } = new();
        public IdiomaticText Description { get; set; } = new();
        public bool Filtering { get; set; }
        public List<Guid> Tags { get; set; } = new();
    }
}
