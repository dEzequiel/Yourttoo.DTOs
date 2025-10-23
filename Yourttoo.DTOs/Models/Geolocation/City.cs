using Yourttoo.DTOs.Common.Constants;
namespace Yourttoo.DTOs.Models.Geolocation
{
    public class City : Geolocation
    {
        public Guid? CountryId { get; set; }
        public string? CountryCode { get; set; }
        public Guid? ZoneId { get; set; }

        // public Guid? RegionId { get; set; }
        
        // Navigation properties
        public Country? Country { get; set; }
        public Zone? Zone { get; set; }
        // public Region? Region { get; set; }
        public City() {
            Category = GeolocationCategories.City;
        }
    }
}