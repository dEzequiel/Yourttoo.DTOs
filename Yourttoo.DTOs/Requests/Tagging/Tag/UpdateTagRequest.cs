namespace Yourttoo.DTOs.Requests.Tagging.Tag
{
    /// <summary>
    /// Request for updating an existing Tag
    /// </summary>
    public class UpdateTagRequest : TagRequestPayload
    {
        /// <summary>
        /// ID of the Tag to update
        /// </summary>
        public Guid Id { get; set; } = Guid.Empty;
    }
}
