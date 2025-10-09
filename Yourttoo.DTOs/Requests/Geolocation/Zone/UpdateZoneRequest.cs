using Yourttoo.DTOs.Common;
using Yourttoo.DTOs.Shared.Features;

namespace Yourttoo.DTOs.Requests.Geolocation.Zone
{
    public class UpdateZoneRequest : RequestPayloadBase
    {
        public Guid Id { get; set; } = Guid.Empty;
        public long Latitude { get; set; }
        public long Longitude { get; set; }
        public double AveragePrice { get; set; }
        public List<MultiLanguageText> Name { get; set; } = new();
        public List<MultiLanguageText>? Description { get; set; } = new();
        public string ImageUrl { get; set; } = string.Empty;
        public string IconUrl { get; set; } = string.Empty;
        public string BackgroundColor { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public string ThumbnailUrl { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public string PromotionArea { get; set; } = string.Empty;
        public int PromotionAreaPriority { get; set; }

        public UpdateZoneRequest() { }
    }
}
