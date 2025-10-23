using Yourttoo.DTOs.Common;
using Yourttoo.DTOs.Shared.Features;

namespace Yourttoo.DTOs.Requests.Tagging.Tag
{
    /// <summary>
    /// Request for partially updating a Tag (PATCH operation)
    /// All properties are optional for partial updates
    /// </summary>
    public class PatchTagRequest : RequestPayloadBase
    {
        /// <summary>
        /// ID of the Tag to update
        /// </summary>
        public Guid Id { get; set; } = Guid.Empty;
        
        public string? Key { get; set; } = null;
        public string? Value { get; set; } = null;
        public string? Category { get; set; } = null;
        public string? SubCategory { get; set; } = null;
        public string? Type { get; set; } = null;
        public MultiLanguageText? Name { get; set; } = null;
        public MultiLanguageText? Description { get; set; } = null;
        public string? IconUrl { get; set; } = null;
        public string? ImageUrl { get; set; } = null;
        public string? Status { get; set; } = null;
    }
}
