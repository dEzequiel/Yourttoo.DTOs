namespace Yourttoo.DTOs.Models
{
    /// <summary>
    /// Base interface for all entities in the system
    /// Provides common properties for identification, audit, and soft delete
    /// </summary>
    public abstract class IEntity
    {
        public Guid Id { get; set; }
        public string CreatedBy { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
