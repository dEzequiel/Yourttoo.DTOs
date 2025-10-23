using Yourttoo.DTOs.Shared.Features;

namespace Yourttoo.DTOs.Requests.Tagging.Tag
{
    /// <summary>
    /// Request for deleting multiple Tags in a single operation
    /// </summary>
    public class BulkDeleteTagRequest : RequestPayloadBase
    {
        /// <summary>
        /// List of Tag IDs to delete
        /// </summary>
        public List<Guid> TagIds { get; set; } = new List<Guid>();
    
    }
}
