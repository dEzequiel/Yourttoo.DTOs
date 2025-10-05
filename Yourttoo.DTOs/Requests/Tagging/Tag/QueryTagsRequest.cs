using Yourttoo.DTOs.Shared.Features;

namespace Yourttoo.DTOs.Requests.Tagging.Tag
{
    public class QueryTagsRequest : QueryPayloadBase
    {
        /// <summary>
        /// Search term to filter tags by name, description, or code
        /// </summary>
        public string? SearchTerm { get; set; } = null;

        /// <summary>
        /// Filter by tag status
        /// </summary>
        public string? Status { get; set; } = null;

        /// <summary>
        /// Filter by category ID
        /// </summary>
        public Guid? CategoryId { get; set; } = null;

        /// <summary>
        /// Filter by multiple category IDs
        /// </summary>
        public List<Guid>? CategoryIds { get; set; } = null;

        /// <summary>
        /// Filter by tag code
        /// </summary>
        public string? Code { get; set; } = null;

        /// <summary>
        /// Filter by slug
        /// </summary>
        public string? Slug { get; set; } = null;

        public QueryTagsRequest() { }
    }
}
