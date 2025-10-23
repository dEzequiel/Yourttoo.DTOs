using Yourttoo.DTOs.Common;

namespace Yourttoo.DTOs.DTOs.Geolocation
{
    public abstract class GeolocationDTO : DataTransferObject
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public decimal AveragePrice { get; set; }
        public string Language { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
        public string IconUrl { get; set; } = string.Empty;
        public string BackgroundColor { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public string ThumbnailUrl { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
    }
}
