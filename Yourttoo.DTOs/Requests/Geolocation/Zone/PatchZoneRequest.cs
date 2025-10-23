using Yourttoo.DTOs.Common;
using Yourttoo.DTOs.Shared.Features;

namespace Yourttoo.DTOs.Requests.Geolocation.Zone
{
    public class PatchZoneRequest : RequestPayloadBase
    {
        public Guid Id { get; set; } = Guid.Empty;
        public double? Latitude { get; set; } = null;
        public double? Longitude { get; set; } = null;
        public decimal? AveragePrice { get; set; } = null;
        public MultiLanguageText? Name { get; set; } = null;
        public MultiLanguageText? Description { get; set; } = null;
        public string? ImageUrl { get; set; } = null;
        public string? IconUrl { get; set; } = null;
        public string? BackgroundColor { get; set; } = null;
        public string? Status { get; set; } = null;
        public string? ThumbnailUrl { get; set; } = null;
        public string? Category { get; set; } = null;
        public string? PromotionArea { get; set; } = null;
        public int? PromotionAreaPriority { get; set; } = null;

    }
}
