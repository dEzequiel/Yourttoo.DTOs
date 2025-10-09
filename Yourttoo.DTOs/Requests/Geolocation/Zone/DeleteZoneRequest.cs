using Yourttoo.DTOs.Shared.Features;

namespace Yourttoo.DTOs.Requests.Geolocation.Zone
{
    public class DeleteZoneRequest : RequestPayloadBase
    {
        public Guid Id { get; set; } = Guid.Empty;

        public DeleteZoneRequest() { }
    }
}
