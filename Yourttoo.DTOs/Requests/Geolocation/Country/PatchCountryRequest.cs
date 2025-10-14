using Yourttoo.DTOs.Common;
using Yourttoo.DTOs.Shared.Features;

namespace Yourttoo.DTOs.Requests.Geolocation.Country
{
    public class PatchCountryRequest : RequestPayloadBase
    {
        public Guid Id { get; set; } = Guid.Empty;
        public long? Latitude { get; set; } = null;
        public long? Longitude { get; set; } = null;
        public double? AveragePrice { get; set; } = null;
        public List<MultiLanguageText>? Name { get; set; } = null;
        public List<MultiLanguageText>? Description { get; set; } = null;
        public string? ImageUrl { get; set; } = null;
        public string? IconUrl { get; set; } = null;
        public string? BackgroundColor { get; set; } = null;
        public string? Status { get; set; } = null;
        public string? ThumbnailUrl { get; set; } = null;
        public string? Category { get; set; } = null;
        public string? Currency { get; set; } = null;
        public string? CurrencySymbol { get; set; } = null;
        public string? Language { get; set; } = null;
        public string? LanguageCode { get; set; } = null;
        public string? TimeZone { get; set; } = null;

        public PatchCountryRequest() { }
    }
}
