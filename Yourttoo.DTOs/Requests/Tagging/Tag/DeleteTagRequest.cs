using Yourttoo.DTOs.Shared.Features;

namespace Yourttoo.DTOs.Requests.Tagging.Tag
{
    /// <summary>
    /// Request for deleting a Tag
    /// </summary>
    public class DeleteTagRequest : RequestPayloadBase
    {
        /// <summary>
        /// ID of the Tag to delete
        /// </summary>
        public Guid Id { get; set; } = Guid.Empty;
        
        /// <summary>
        /// Whether to perform a soft delete (mark as deleted) or hard delete
        /// Default is soft delete
        /// </summary>
        public bool SoftDelete { get; set; } = true;
    }
}
