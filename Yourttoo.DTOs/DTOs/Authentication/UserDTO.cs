using Yourttoo.DTOs.Common;

namespace Yourttoo.DTOs.DTOs.Authentication
{
    public class UserDTO : IDataTransferObject
    {
        public Guid Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string? Avatar { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<RoleDTO> Roles { get; set; } = new List<RoleDTO>();
    }
}
