using Yourttoo.DTOs.Common;
using Yourttoo.DTOs.Shared.Features;

namespace Yourttoo.DTOs.Requests.Tagging
{
    /// <summary>
    /// Base class for Tag request payloads
    /// Contains common properties for Tag operations
    /// </summary>
    public class TagRequestPayload : RequestPayloadBase
    {
        public string Key { get; set; } = string.Empty;
        public string Value { get; set; } = string.Empty;
        public string? Category { get; set; } = null;
        public string? SubCategory { get; set; } = null;
        public string Type { get; set; } = string.Empty;
        public MultiLanguageText Name { get; set; } = new();
        public MultiLanguageText Description { get; set; } = new();
        public string? IconUrl { get; set; } = null;
        public string? ImageUrl { get; set; } = null;
        public string Status { get; set; } = "Active";
    }
}
