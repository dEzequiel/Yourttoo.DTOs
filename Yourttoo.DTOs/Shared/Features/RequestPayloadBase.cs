namespace Yourttoo.DTOs.Shared.Features
{
    /// <summary>
    /// Base class for payloads of a Request
    /// Contains common properties that all requests should have
    /// </summary>
    public abstract class RequestPayloadBase
    {
        /// <summary>
        /// User who created or modified the entity
        /// Used for audit purposes
        /// </summary>
        public string? CreatedBy { get; set; } = null;

        /// <summary>
        /// Language code for the request (e.g., "es", "en", "fr")
        /// Used to specify which language version of multi-language content to return
        /// </summary>
        public string Language { get; set; } = "es";
    }
}
