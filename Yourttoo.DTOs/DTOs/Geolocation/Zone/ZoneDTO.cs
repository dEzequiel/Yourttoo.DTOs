
namespace Yourttoo.DTOs.DTOs.Geolocation.Zone
{
    public class ZoneDTO
    {
        public Guid Id { get; set; }
        public string Language { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;

        public string PromotionArea { get; set; } = string.Empty;
        public int PromotionAreaPriority { get; set; } = 0;
    }
}
