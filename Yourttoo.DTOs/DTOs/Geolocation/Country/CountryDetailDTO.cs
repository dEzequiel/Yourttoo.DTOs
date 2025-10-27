using Yourttoo.DTOs.Common;

namespace Yourttoo.DTOs.DTOs.Geolocation.Country
{
    public class CountryDetailDTO : GeolocationDetailDTO
    {
        public string Currency { get; set; } = string.Empty;
        public string CurrencySymbol { get; set; } = string.Empty;
        public string CountryLanguage { get; set; } = string.Empty;
        public string LanguageCode { get; set; } = string.Empty;
        public string TimeZone { get; set; } = string.Empty;
        public string? Continent { get; set; } = null;
        public Guid? ZoneId { get; set; } = null;
    }
}
