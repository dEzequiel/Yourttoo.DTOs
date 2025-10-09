using Yourttoo.DTOs.Common;

namespace Yourttoo.DTOs.DTOs.Geolocation.Airport
{
    public class AirportDTO : DataTransferObject
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
        public City.CityDTO City { get; set; } = new();
        public Zone.ZoneDTO Zone { get; set; } = new();
        public Country.CountryDTO Country { get; set; } = new();
        public string CountryCode { get; set; } = string.Empty;
        public Region.RegionDTO Region { get; set; } = new();
        public string IATACode { get; set; } = string.Empty;
        public string ICAOCode { get; set; } = string.Empty;
        public string TimeZone { get; set; } = string.Empty;
        public int GMTOffset { get; set; } = 0;
        public int DSTOffset { get; set; } = 0;
        public string CreatedBy { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
