using Yourttoo.DTOs.Shared.Features;

namespace Yourttoo.DTOs.Requests.Geolocation.Country
{
    public class QueryCountryRequest : GeolocationQueryPayload
    {
        public string? Currency { get; set; } = null;
        public string? LanguageCode { get; set; } = null;
        public string? TimeZone { get; set; } = null;

    }
}
