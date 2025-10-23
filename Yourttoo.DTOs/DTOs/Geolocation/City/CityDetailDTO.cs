using Yourttoo.DTOs.Common;

namespace Yourttoo.DTOs.DTOs.Geolocation.City
{
    public class CityDetailDTO : GeolocationDetailDTO
    {
        public Guid? CountryId { get; set; }
        public Guid? ZoneId { get; set; }
    }
}
