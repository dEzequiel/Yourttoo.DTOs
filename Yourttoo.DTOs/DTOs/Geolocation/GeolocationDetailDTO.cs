using Yourttoo.DTOs.Common;

namespace Yourttoo.DTOs.DTOs.Geolocation
    {
    public abstract class GeolocationDetailDTO : DataTransferObject
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public decimal AveragePrice { get; set; }
        public MultiLanguageText Name { get; set; } = new();
        public MultiLanguageText Description { get; set; } = new();
        public string ImageUrl { get; set; } = string.Empty;
        public string IconUrl { get; set; } = string.Empty;
        public string BackgroundColor { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public string ThumbnailUrl { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
    }
}
