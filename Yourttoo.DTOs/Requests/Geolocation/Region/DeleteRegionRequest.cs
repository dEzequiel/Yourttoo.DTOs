using Yourttoo.DTOs.Shared.Features;

namespace Yourttoo.DTOs.Requests.Geolocation.Region
{
    public class DeleteRegionRequest : RequestPayloadBase
    {
        public Guid Id { get; set; }
    }
}
