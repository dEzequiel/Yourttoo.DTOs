using Yourttoo.DTOs.Requests.Geolocation;

namespace Yourttoo.DTOs.Requests.Geolocation.Country
{
    public class PatchCountryRequest : GeolocationRequestPayload
    {
        public Guid Id { get; set; } = Guid.Empty;
        public string? Currency { get; set; } = null;
        public string? CurrencySymbol { get; set; } = null;
        public string? LanguageCode { get; set; } = null;
        public string? TimeZone { get; set; } = null;
        public string? Continent { get; set; } = null;
        public Guid? ZoneId { get; set; } = null;
    }
}
