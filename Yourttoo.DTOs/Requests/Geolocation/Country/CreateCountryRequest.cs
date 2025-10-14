using Yourttoo.DTOs.Common;
using Yourttoo.DTOs.Shared.Features;
using Yourttoo.DTOs.Common.Constants;

namespace Yourttoo.DTOs.Requests.Geolocation.Country
{
    public class CreateCountryRequest : RequestPayloadBase
    {
        public long Latitude { get; set; }
        public long Longitude { get; set; }
        public double AveragePrice { get; set; }
        public List<MultiLanguageText> Name { get; set; } = new();
        public List<MultiLanguageText>? Description { get; set; } = new();
        public string ImageUrl { get; set; } = string.Empty;
        public string IconUrl { get; set; } = string.Empty;
        public string BackgroundColor { get; set; } = string.Empty;
        public string Status { get; set; } = "active";
        public string ThumbnailUrl { get; set; } = string.Empty;
        public string Category { get; set; } = GeolocationCategories.Country;
        public string Currency { get; set; } = string.Empty;
        public string CurrencySymbol { get; set; } = string.Empty;
        public string Language { get; set; } = string.Empty;
        public string LanguageCode { get; set; } = string.Empty;
        public string TimeZone { get; set; } = string.Empty;

        public CreateCountryRequest() { }
    }
}
