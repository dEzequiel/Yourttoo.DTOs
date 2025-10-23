using Yourttoo.DTOs.Common;
using Yourttoo.DTOs.DTOs.Geolocation.Country;
using Yourttoo.DTOs.DTOs.Geolocation.Zone;

namespace Yourttoo.DTOs.DTOs.Geolocation.Region
{
    public class RegionDTO : GeolocationDTO
    {
        public Guid? CountryId { get; set; }
        public Guid? ZoneId { get; set; }
    }
}
