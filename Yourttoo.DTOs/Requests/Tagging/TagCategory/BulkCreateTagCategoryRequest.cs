
using Yourttoo.DTOs.Shared.Features;

namespace Yourttoo.DTOs.Requests.Tagging.TagCategory
{
    public class BulkCreateTagCategoryRequest : RequestPayloadBase
    {
        public List<CreateTagCategoryRequest> Categories { get; set; } = new();
        public BulkCreateTagCategoryRequest() { }
    }
}
