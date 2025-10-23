namespace Yourttoo.DTOs.Requests.Geolocation.City
{
    public class CreateCityRequest : GeolocationRequestPayload
    {
        public Guid? CountryId { get; set; }
        public string? CountryCode { get; set; }
        public Guid? ZoneId { get; set; }
    }
}
