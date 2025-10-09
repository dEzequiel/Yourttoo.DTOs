using Yourttoo.DTOs.Common;
using Yourttoo.DTOs.Shared.Features;
using Yourttoo.DTOs.Common.Constants;

namespace Yourttoo.DTOs.Requests.Geolocation.Zone
{
    public class CreateZoneRequest : RequestPayloadBase
    {
        public long Latitude { get; set; }
        public long Longitude { get; set; }
        public double AveragePrice { get; set; }
        public List<MultiLanguageText> Name { get; set; } = new();
        public List<MultiLanguageText>? Description { get; set; } = new();
        public string ImageUrl { get; set; } = string.Empty;
        public string IconUrl { get; set; } = string.Empty;
        public string BackgroundColor { get; set; } = string.Empty;
        public string Status { get; set; } = "active";
        public string ThumbnailUrl { get; set; } = string.Empty;
        public string Category { get; set; } = GeolocationCategories.Zone;
        public string PromotionArea { get; set; } = string.Empty;
        public int PromotionAreaPriority { get; set; } = 0;

        public CreateZoneRequest() { }
    }
}
