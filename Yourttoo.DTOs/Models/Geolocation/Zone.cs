using Yourttoo.DTOs.Common.Constants;
namespace Yourttoo.DTOs.Models.Geolocation
{
    public class Zone : Geolocation
    {
        public string PromotionArea { get; set; }
        public int PromotionAreaPriority { get; set; }

        // Countries reference
        public List<Country> Countries { get; set; } = new();
        public Zone() {
            PromotionArea = "General";
            PromotionAreaPriority = 0;
            Category = GeolocationCategories.Zone;
        }
    }
}