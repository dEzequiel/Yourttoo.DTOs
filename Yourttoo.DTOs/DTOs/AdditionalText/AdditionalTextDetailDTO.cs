using Yourttoo.DTOs.Common;

namespace Yourttoo.DTOs.DTOs.AdditionalText
{
    public class AdditionalTextDetailDTO : IDataTransferObject
    {
        public Guid Id { get; set; } = Guid.Empty;
        public List<MultiLanguageText> Title { get; set; } = new();
        public List<MultiLanguageText>? Content { get; set; } = new();
        public string Status { get; set; } = string.Empty;
    }
}
