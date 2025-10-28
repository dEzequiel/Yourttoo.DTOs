using Yourttoo.DTOs.Common;

namespace Yourttoo.DTOs.DTOs.Geolocation.City
{
    public class CityDTO : DataTransferObject
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public string BackgroundColor { get; set; } = string.Empty;

        public Guid? CountryId { get; set; }
        public Guid? ZoneId { get; set; }
    }
}
