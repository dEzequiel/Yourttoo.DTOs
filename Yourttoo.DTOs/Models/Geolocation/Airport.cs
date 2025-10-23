using Yourttoo.DTOs.Common.Constants;
namespace Yourttoo.DTOs.Models.Geolocation
{
    public class Airport : Geolocation
    {
        public string? IataCode { get; set; }
        public string? IcaoCode { get; set; }
        public string? TimeZone { get; set; }
        public int GMTOffset { get; set; } = 0; // Offset from GMT in hours
        public int DSTOffset { get; set; } = 0; // Offset during Daylight Saving Time in hours
        public Guid? CityId { get; set; }
        public Guid? ZoneId { get; set; }
        public Guid? CountryId { get; set; }

        // Navigation properties
        public City? City { get; set; }
        public Zone? Zone { get; set; }
        public Country? Country { get; set; }

        public Airport() {
            Category = GeolocationCategories.Airport;
        }
    }
}