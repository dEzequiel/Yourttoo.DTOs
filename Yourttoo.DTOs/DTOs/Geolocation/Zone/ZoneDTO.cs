using Yourttoo.DTOs.Common;

namespace Yourttoo.DTOs.DTOs.Geolocation.Zone
{
    public class ZoneDTO : DataTransferObject
    {
        public long Latitude { get; set; }
        public long Longitude { get; set; }
        public double AveragePrice { get; set; }
        public MultiLanguageText Name { get; set; } = new();
        public MultiLanguageText? Description { get; set; } = null;
        public string ImageUrl { get; set; } = string.Empty;
        public string BackgroundColor { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public string ThumbnailUrl { get; set; } = string.Empty;
        public int PromotionAreaPriority { get; set; }
    }
}
