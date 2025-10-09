using Yourttoo.DTOs.Common;
using Yourttoo.DTOs.Shared.Features;

namespace Yourttoo.DTOs.Requests.Tagging.TagCategory
{
    public class CreateTagCategoryRequest : RequestPayloadBase
    {
        public string Code { get; set; } = string.Empty;
        public List<MultiLanguageText> Name { get; set; } = new();
        public List<MultiLanguageText> Description { get; set; } = new();
        public bool Filtering { get; set; }

        public CreateTagCategoryRequest() { }
    }
}
