namespace Yourttoo.DTOs.Requests.Geolocation.Region
{
    public class UpdateRegionRequest : GeolocationRequestPayload
    {
        public Guid Id { get; set; }
        public Guid? CountryId { get; set; }
        public Guid? ZoneId { get; set; }
    }
}
