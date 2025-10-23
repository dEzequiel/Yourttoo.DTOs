using Yourttoo.DTOs.Common;

namespace Yourttoo.DTOs.DTOs.Geolocation.Airport
{
    public class AirportDTO : GeolocationDTO
    {
        public Guid? CityId { get; set; }
        public Guid? ZoneId { get; set; }
        public Guid? CountryId { get; set; }
        public string? IATACode { get; set; }
        public string? ICAOCode { get; set; }
        public string? TimeZone { get; set; }
        public int? GMTOffset { get; set; }
        public int? DSTOffset { get; set; }
    }
}
