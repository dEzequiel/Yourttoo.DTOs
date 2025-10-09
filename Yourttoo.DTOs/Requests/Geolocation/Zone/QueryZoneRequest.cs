using Yourttoo.DTOs.Shared.Features;

namespace Yourttoo.DTOs.Requests.Geolocation.Zone
{
    public class QueryZoneRequest : QueryPayloadBase
    {
        public string? SearchTerm { get; set; } = null;
        public string? Status { get; set; } = null;
        public string? Category { get; set; } = null;
        public string? PromotionArea { get; set; } = null;
        public int? MinPromotionAreaPriority { get; set; } = null;
        public int? MaxPromotionAreaPriority { get; set; } = null;
        public double? MinAveragePrice { get; set; } = null;
        public double? MaxAveragePrice { get; set; } = null;
        public long? MinLatitude { get; set; } = null;
        public long? MaxLatitude { get; set; } = null;
        public long? MinLongitude { get; set; } = null;
        public long? MaxLongitude { get; set; } = null;

        public QueryZoneRequest() { }
    }
}
