using Yourttoo.DTOs.Common;

namespace Yourttoo.DTOs.DTOs.AdditionalText
{
    public class AdditionalTextDetailDTO : IDataTransferObject
    {
        public Guid Id { get; set; } = Guid.Empty;
        public MultiLanguageText Title { get; set; } = new();
        public MultiLanguageText? Content { get; set; } = new();
        public string Status { get; set; } = string.Empty;
    }
}
