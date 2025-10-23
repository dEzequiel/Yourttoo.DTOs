using Yourttoo.DTOs.Shared.Features;

namespace Yourttoo.DTOs.Requests.Geolocation.Airport
{
    public class DeleteAirportRequest : RequestPayloadBase
    {
        public Guid Id { get; set; }
    }
}
