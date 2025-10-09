using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yourttoo.DTOs.Common;

namespace Yourttoo.DTOs.DTOs.Tagging.TagCategory
{
    public class TagCategoryDetailDTO : IDataTransferObject
    {
        public Guid Id { get; set; } = Guid.Empty;
        public string Code { get; set; } = string.Empty;
        public List<MultiLanguageText> Name { get; set; } = new();
        public List<MultiLanguageText> Description { get; set; } = new();
        public bool Filtering { get; set; }
        public List<Guid> Tags { get; set; } = new();
    }
}
