using Yourttoo.DTOs.Common.Constants;
namespace Yourttoo.DTOs.Models.Geolocation
{
    public class City : Geolocation
    {
        public Region Region { get; set; } = new();
        public Country Country { get; set; } = new();
        public string CountryCode { get; set; } = string.Empty;
        public Zone Zone { get; set; } = new();

        public City() {
            Category = GeolocationCategories.City;
        }
    }
}