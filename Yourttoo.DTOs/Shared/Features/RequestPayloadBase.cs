
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

        public string? UpdatedBy { get; set; } = null;
    }
}
