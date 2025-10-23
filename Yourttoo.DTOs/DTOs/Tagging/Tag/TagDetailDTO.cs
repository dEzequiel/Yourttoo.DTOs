using Yourttoo.DTOs.Common;

namespace Yourttoo.DTOs.DTOs.Tagging.Tag
{
    public class TagDetailDTO : DataTransferObject
    {
        public string? Key { get; set; }
        public string? Value { get; set; }
        public string? Category { get; set; }
        public string? SubCategory { get; set; }
        public string Type { get; set; }
        public MultiLanguageText? Name { get; set; }
        public MultiLanguageText? Description { get; set; }
        public string? IconUrl { get; set; }
        public string? ImageUrl { get; set; }
        public string Status { get; set; } 
        public string Language { get; set; } 
    }
}
