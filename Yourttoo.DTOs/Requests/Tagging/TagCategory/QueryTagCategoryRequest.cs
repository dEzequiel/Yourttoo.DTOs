using Yourttoo.DTOs.Shared.Features;

namespace Yourttoo.DTOs.Requests.Tagging.TagCategory
{
    public class QueryTagCategoryRequest : QueryPayloadBase
    {
        /// <summary>
        /// Search term to filter
        /// </summary>
        public string? SearchTerm { get; set; } = null;
        
        /// <summary>
        /// Filter by tag category code
        /// </summary>
        public string? Code { get; set; } = null;
        
        /// <summary>
        /// Filter categories that are used for filtering
        /// </summary>
        public bool? IsFilterable { get; set; } = null;

        public QueryTagCategoryRequest() { }
    }
}
