using Yourttoo.DTOs.Common;

namespace Yourttoo.DTOs.DTOs.Geolocation.Country
{
    public class CountryDTO : GeolocationDTO
    {
        public string Currency { get; set; } = string.Empty;
        public string CurrencySymbol { get; set; } = string.Empty;
        public string LanguageCode { get; set; } = string.Empty;
        public string TimeZone { get; set; } = string.Empty;
        public string? Continent { get; set; } 
        public Guid? ZoneId { get; set; } = null;
    }
}
