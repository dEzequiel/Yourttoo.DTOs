using Yourttoo.DTOs.Shared.Features;

namespace Yourttoo.DTOs.Requests.Tagging.TagCategory
{
    public class DeleteTagCategoryRequest : RequestPayloadBase
    {
        public Guid Id { get; set; }
    }
}
