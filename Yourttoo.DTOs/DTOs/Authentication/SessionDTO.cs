using Yourttoo.DTOs.Common;

namespace Yourttoo.DTOs.DTOs.Authentication
{
    public class SessionDTO : IDataTransferObject
    {
        public Guid Id { get; set; } 
        public string SessionId { get; set; } = string.Empty;
        public string UserId { get; set; } = string.Empty;
        public DateTime ExpiresAt { get; set; }
        public bool IsActive { get; set; }
    }
}
