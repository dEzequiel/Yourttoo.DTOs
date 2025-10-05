using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yourttoo.DTOs.Common;

namespace Yourttoo.DTOs.DTOs.Tagging.Tag
{
    public class TagDetailDTO : IDataTransferObject
    {
        public Guid Id { get; set; } = Guid.Empty;
        public string Code { get; set; } = string.Empty;
        public List<IdiomaticText> Name { get; set; } = new();
        public List<IdiomaticText> Description { get; set; } = new();
        public string Slug { get; set; } = string.Empty;
        public List<IdiomaticText> Label { get; set; } = new();
        public string Status { get; set; } = string.Empty;
        public string Icon { get; set; } = string.Empty;
        public List<Guid> Categories { get; set; } = new();
        public TagDetailDTO() { }
    }
}
