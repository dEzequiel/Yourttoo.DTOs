using Yourttoo.DTOs.Common;

namespace Yourttoo.DTOs.Models.Geolocation
{
    public abstract class Geolocation : IEntity
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public decimal AveragePrice { get; set; }
        public MultiLanguageText Name { get; set; } = new MultiLanguageText();
        public MultiLanguageText Description { get; set; } = new MultiLanguageText();
        public string? ImageUrl { get; set; }
        public string? IconUrl { get; set; }
        public string? BackgroundColor { get; set; }
        public string? Status { get; set; }
        public string? ThumbnailUrl { get; set; }
        public string? Category { get; set; }
    }
}