using Yourttoo.DTOs.Common;

namespace Yourttoo.DTOs.Models.Authentication
{
    public class Session 
    {
        public Guid Id { get; set; }
        public string SessionId { get; set; } = string.Empty;
        public Guid UserId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ExpiresAt { get; set; }
        public bool IsActive { get; set; } = true;
        public string? IpAddress { get; set; }
        public string? UserAgent { get; set; }
        
        // Navigation property
        public virtual User User { get; set; } = null!;
    }
}
