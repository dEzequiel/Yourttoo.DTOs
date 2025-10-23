namespace Yourttoo.DTOs.DTOs
{
    public abstract class DataTransferObject
    {
        public Guid Id { get; set; }
        public string? CreatedBy { get; set; } 
        public DateTime? CreatedAt { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }

    }
}