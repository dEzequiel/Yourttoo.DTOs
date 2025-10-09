using Yourttoo.DTOs.Common;

namespace Yourttoo.DTOs.DTOs.Geolocation
{
    public class GeolocationDTO : DataTransferObject
    {
        public long Latitude { get; set; }
        public long Longitude { get; set; }
        public double AveragePrice { get; set; }
        public MultiLanguageText Name { get; set; } = new();
        public MultiLanguageText? Description { get; set; } = null;
        public string ImageUrl { get; set; } = string.Empty;
        public string IconUrl { get; set; } = string.Empty;
        public string BackgroundColor { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public string ThumbnailUrl { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public string CreatedBy { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
