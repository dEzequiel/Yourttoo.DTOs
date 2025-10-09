using Yourttoo.DTOs.Common.Constants;
namespace Yourttoo.DTOs.Models.Geolocation
{
    public class Zone : Geolocation
    {
        public string PromotionArea { get; set; } = string.Empty;
        public int PromotionAreaPriority { get; set; }

        public Zone() {
            Category = GeolocationCategories.Zone;
        }
    }
}