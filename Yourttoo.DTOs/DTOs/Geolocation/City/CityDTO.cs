using Yourttoo.DTOs.Common;

namespace Yourttoo.DTOs.DTOs.Geolocation.City
{
    public class CityDTO : GeolocationDTO
    {
        public Guid? CountryId { get; set; }
        public Guid? ZoneId { get; set; }
    }
}
