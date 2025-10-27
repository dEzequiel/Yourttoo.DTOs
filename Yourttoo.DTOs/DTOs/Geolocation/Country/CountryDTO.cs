using Yourttoo.DTOs.Common;

namespace Yourttoo.DTOs.DTOs.Geolocation.Country
{
    public class CountryDTO : DataTransferObject
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Currency { get; set; } = string.Empty;
        public string CurrencySymbol { get; set; } = string.Empty;
        public string CountryLanguage { get; set; } = string.Empty;
        public string? Continent { get; set; } 
        public string ImageUrl { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public Guid? ZoneId { get; set; } = null;
    }
}
