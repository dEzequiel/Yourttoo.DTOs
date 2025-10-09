using Yourttoo.DTOs.Common.Constants;
namespace Yourttoo.DTOs.Models.Geolocation
{
    public class Airport : Geolocation
    {
        public City City { get; set; } = new();
        public Zone Zone { get; set; } = new();
        public Country Country { get; set; } = new();               
        public string CountryCode { get; set; } = string.Empty;
        public Region Region { get; set; } = new();

        public string IATACode { get; set; } = string.Empty;
        public string ICAOCode { get; set; } = string.Empty;
        public string TimeZone { get; set; } = string.Empty;
        public int GMTOffset { get; set; } = 0; // Offset from GMT in hours
        public int DSTOffset { get; set; } = 0; // Offset during Daylight Saving Time in hours

        public Airport() {
            Category = GeolocationCategories.Airport;
        }
    }
}