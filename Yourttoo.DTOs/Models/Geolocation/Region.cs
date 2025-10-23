using Yourttoo.DTOs.Common.Constants;
namespace Yourttoo.DTOs.Models.Geolocation
{
    public class Region : Geolocation
    {
        public Guid? CountryId { get; set; }
        public Guid? ZoneId { get; set; }

        // Navigation properties
        public Country? Country { get; set; }
        public Zone? Zone { get; set; }

        public Region() {
            Category = GeolocationCategories.Region;
        }
    }
}