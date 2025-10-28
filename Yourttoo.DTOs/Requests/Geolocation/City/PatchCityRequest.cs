namespace Yourttoo.DTOs.Requests.Geolocation.City
{
    public class PatchCityRequest : GeolocationRequestPayload
    {
        public Guid Id { get; set; }
        public Guid? CountryId { get; set; }
        public string? CountryCode { get; set; }
        public Guid? ZoneId { get; set; }
    }
}
