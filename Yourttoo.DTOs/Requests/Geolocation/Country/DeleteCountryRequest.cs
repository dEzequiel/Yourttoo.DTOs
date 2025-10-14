using Yourttoo.DTOs.Shared.Features;

namespace Yourttoo.DTOs.Requests.Geolocation.Country
{
    public class DeleteCountryRequest : RequestPayloadBase
    {
        public Guid Id { get; set; } = Guid.Empty;

        public DeleteCountryRequest() { }
    }
}
