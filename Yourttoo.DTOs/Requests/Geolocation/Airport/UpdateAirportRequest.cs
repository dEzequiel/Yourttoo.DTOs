namespace Yourttoo.DTOs.Requests.Geolocation.Airport
{
    public class UpdateAirportRequest : GeolocationRequestPayload
    {
        public Guid Id { get; set; }
        public Guid? CityId { get; set; }
        public Guid? ZoneId { get; set; }
        public Guid? CountryId { get; set; }
        public string? IataCode { get; set; }
        public string? IcaoCode { get; set; }
        public string? TimeZone { get; set; }
        public int? GmtOffset { get; set; }
        public int? DstOffset { get; set; }
    }
}
