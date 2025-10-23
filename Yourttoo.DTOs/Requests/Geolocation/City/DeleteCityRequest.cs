using Yourttoo.DTOs.Shared.Features;

namespace Yourttoo.DTOs.Requests.Geolocation.City
{
    public class DeleteCityRequest : RequestPayloadBase
    {
        public Guid Id { get; set; }
    }
}
