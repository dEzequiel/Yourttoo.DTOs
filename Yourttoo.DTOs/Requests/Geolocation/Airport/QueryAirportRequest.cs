namespace Yourttoo.DTOs.Requests.Geolocation.Airport
{
    public class QueryAirportRequest : GeolocationQueryPayload
    {
        public Guid? CityId { get; set; }
        public Guid? ZoneId { get; set; }
        public Guid? CountryId { get; set; }
        public string? IataCode { get; set; }
        public string? IcaoCode { get; set; }
        public string? TimeZone { get; set; }
    }
}
