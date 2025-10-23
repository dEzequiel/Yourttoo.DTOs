using Yourttoo.DTOs.Shared.Features;

namespace Yourttoo.DTOs.Requests.Geolocation
{
    public class GeolocationQueryPayload : QueryPayloadBase
    {
        public string? SearchTerm { get; set; } = null;
        public string? Status { get; set; } = null;
        public string? Category { get; set; } = null;
        public decimal? MinAveragePrice { get; set; } = null;
        public decimal? MaxAveragePrice { get; set; } = null;
        public double? MinLatitude { get; set; } = null;
        public double? MaxLatitude { get; set; } = null;
        public double? MinLongitude { get; set; } = null;
        public double? MaxLongitude { get; set; } = null;
    }
}