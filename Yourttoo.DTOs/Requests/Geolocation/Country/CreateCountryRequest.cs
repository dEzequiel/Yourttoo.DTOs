
namespace Yourttoo.DTOs.Requests.Geolocation.Country
{
    public class CreateCountryRequest : GeolocationRequestPayload
    {
        public string Currency { get; set; } = string.Empty;
        public string CurrencySymbol { get; set; } = string.Empty;
        public string LanguageCode { get; set; } = string.Empty;
        public string TimeZone { get; set; } = string.Empty;
        public string Continent { get; set; } = string.Empty;
        public Guid ZoneId { get; set; } = Guid.Empty;
    }
}
