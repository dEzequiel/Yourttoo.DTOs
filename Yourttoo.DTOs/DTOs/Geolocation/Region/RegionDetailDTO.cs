using Yourttoo.DTOs.Common;

namespace Yourttoo.DTOs.DTOs.Geolocation.Region
{
    public class RegionDetailDTO : GeolocationDetailDTO
    {
        public Guid? CountryId { get; set; }
        public Guid? ZoneId { get; set; }
    }
}
