using Yourttoo.DTOs.Shared.Features;

namespace Yourttoo.DTOs.Requests.Tagging
{
    /// <summary>
    /// Base class for Tag query payloads
    /// Contains common properties for Tag queries and filtering
    /// </summary>
    public class TagQueryPayload : QueryPayloadBase
    {
        public string? SearchTerm { get; set; } = null;
        public string? Key { get; set; } = null;
        public string? Value { get; set; } = null;
        public string? Category { get; set; } = null;
        public string? SubCategory { get; set; } = null;
        public string? Type { get; set; } = null;
        public string? Status { get; set; } = null;
        public string? Language { get; set; } = null;
    }
}
