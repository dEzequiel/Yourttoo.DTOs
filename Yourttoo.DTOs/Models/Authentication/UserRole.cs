using Yourttoo.DTOs.Models;

namespace Yourttoo.DTOs.Models.Authentication
{
    public class UserRole 
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid RoleId { get; set; }
        public DateTime AssignedAt { get; set; }
        public bool IsActive { get; set; } = true;
        
        // Navigation properties
        public virtual User User { get; set; } = null!;
        public virtual Role Role { get; set; } = null!;
    }
}
