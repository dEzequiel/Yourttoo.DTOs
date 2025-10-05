using Yourttoo.DTOs.Common;

namespace Yourttoo.DTOs.Requests.Tagging.TagCategory
{
    public class UpdateTagCategoryRequest
    {
        public Guid Id { get; set; } = Guid.Empty;
        public string Code { get; set; } = string.Empty;
        public List<IdiomaticText> Name { get; set; } = new();
        public List<IdiomaticText> Description { get; set; } = new();
        public bool Filtering { get; set; }
        public UpdateTagCategoryRequest() { }
    }
}
