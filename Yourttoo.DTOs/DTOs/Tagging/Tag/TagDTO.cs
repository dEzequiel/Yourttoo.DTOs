using Yourttoo.DTOs.Common;

namespace Yourttoo.DTOs.DTOs.Tagging.Tag
{
    public class TagDTO : IDataTransferObject
    {
        public Guid Id { get; set; } = Guid.Empty;
        public string Code { get; set; } = string.Empty;
        public IdiomaticText Name { get; set; } = new();
        public IdiomaticText Description { get; set; } = new();
        public string Slug { get; set; } = string.Empty;
        public IdiomaticText Label { get; set; } = new();
        public string Status { get; set; } = string.Empty;
        public string Icon { get; set; } = string.Empty;
        public List<Guid> Categories { get; set; } = new();
        public TagDTO() { }
    }

    public record TagReferenceDTO(IdiomaticText Name);
}
