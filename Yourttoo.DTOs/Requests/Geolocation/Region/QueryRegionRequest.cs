namespace Yourttoo.DTOs.Requests.Geolocation.Region
{
    public class QueryRegionRequest : GeolocationQueryPayload
    {
        public Guid? CountryId { get; set; }
        public Guid? ZoneId { get; set; }
    }
}
