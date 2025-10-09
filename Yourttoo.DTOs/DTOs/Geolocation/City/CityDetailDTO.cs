using Yourttoo.DTOs.Common;

namespace Yourttoo.DTOs.DTOs.Geolocation.City
{
    public class CityDetailDTO : DataTransferObject
    {
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
        public Region.RegionDetailDTO Region { get; set; } = new();
        public Country.CountryDetailDTO Country { get; set; } = new();
        public string CountryCode { get; set; } = string.Empty;
        public Zone.ZoneDetailDTO Zone { get; set; } = new();
    }
}
