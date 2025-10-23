using Yourttoo.DTOs.Shared.Features;

namespace Yourttoo.DTOs.Requests.Tagging.Tag
{
    /// <summary>
    /// Request for creating multiple Tags in a single operation
    /// </summary>
    public class BulkCreateTagRequest : RequestPayloadBase
    {
        /// <summary>
        /// List of Tags to create
        /// </summary>
        public List<CreateTagRequest> Tags { get; set; } = new List<CreateTagRequest>();
        
        /// <summary>
        /// Whether to stop on first error or continue processing all items
        /// Default is to stop on first error
        /// </summary>
        public bool ContinueOnError { get; set; } = false;
    }
}
