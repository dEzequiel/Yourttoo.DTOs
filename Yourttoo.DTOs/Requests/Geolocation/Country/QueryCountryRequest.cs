using Yourttoo.DTOs.Shared.Features;

namespace Yourttoo.DTOs.Requests.Geolocation.Country
{
    public class QueryCountryRequest : QueryPayloadBase
    {
        public string? SearchTerm { get; set; } = null;
        public string? Status { get; set; } = null;
        public string? Category { get; set; } = null;
        public string? Currency { get; set; } = null;
        public string? LanguageCode { get; set; } = null;
        public string? TimeZone { get; set; } = null;
        public double? MinAveragePrice { get; set; } = null;
        public double? MaxAveragePrice { get; set; } = null;
        public long? MinLatitude { get; set; } = null;
        public long? MaxLatitude { get; set; } = null;
        public long? MinLongitude { get; set; } = null;
        public long? MaxLongitude { get; set; } = null;

        public QueryCountryRequest() { }
    }
}
