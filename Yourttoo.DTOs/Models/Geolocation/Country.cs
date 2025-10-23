using Yourttoo.DTOs.Common.Constants;

namespace Yourttoo.DTOs.Models.Geolocation
{
    public class Country : Geolocation
    {
        public string Currency { get; set; } = string.Empty;
        public string CurrencySymbol { get; set; } = string.Empty;
        public string Language { get; set; } = string.Empty;
        public string LanguageCode { get; set; } = string.Empty;
        public string TimeZone { get; set; } = string.Empty;
        public string Continent { get; set; } = string.Empty;

        // Zone 
        public Guid ZoneId { get; set; } = Guid.Empty;
        public Zone? Zone { get; set; }

        public Country() {
            Category = GeolocationCategories.Country;
        }
    }
}