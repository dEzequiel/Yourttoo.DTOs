using Yourttoo.DTOs.Common.Constants;
namespace Yourttoo.DTOs.Models.Geolocation
{
    public class Region : Geolocation
    {
        public Country Country { get; set; } = new();
        public Zone Zone { get; set; } = new();
        public Region() {
            Category = GeolocationCategories.Region;
        }
    }
}