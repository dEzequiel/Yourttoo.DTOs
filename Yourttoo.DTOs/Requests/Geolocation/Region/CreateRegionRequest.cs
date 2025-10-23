namespace Yourttoo.DTOs.Requests.Geolocation.Region
{
    public class CreateRegionRequest : GeolocationRequestPayload
    {
        public Guid? CountryId { get; set; }
        public Guid? ZoneId { get; set; }
    }
}
