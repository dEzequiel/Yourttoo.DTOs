
using Yourttoo.DTOs.Common;
using Yourttoo.DTOs.Common.Constants;

namespace Yourttoo.DTOs.Models.Tagging
{
    public class Tag : IEntity
    {

       public string? Key { get; set; }
        public string? Value { get; set; }

        public string? Category { get; set; }
        public string? SubCategory { get; set; }

        public string Type { get; set; } = TagTypes.SEARCH_KEYWORD; // Default to GENERIC

        public MultiLanguageText? Name { get; set; }
        public MultiLanguageText? Description { get; set; }

        public string? IconUrl { get; set; }
        public string? ImageUrl { get; set; }

        public string Status { get; set; } = ItemStatus.ACTIVE; // Default to active
    }
}
