using Yourttoo.DTOs.Common;
using Yourttoo.DTOs.Shared.Features;

namespace Yourttoo.DTOs.Requests.Geolocation.City
{
    public class PatchCityRequest : RequestPayloadBase
    {
        public Guid Id { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public decimal? AveragePrice { get; set; }
        public MultiLanguageText? Name { get; set; }
        public MultiLanguageText? Description { get; set; }
        public string? ImageUrl { get; set; }
        public string? IconUrl { get; set; }
        public string? BackgroundColor { get; set; }
        public string? Status { get; set; }
        public string? ThumbnailUrl { get; set; }
        public string? Category { get; set; }
        public Guid? CountryId { get; set; }
        public string? CountryCode { get; set; }
        public Guid? ZoneId { get; set; }
    }
}
