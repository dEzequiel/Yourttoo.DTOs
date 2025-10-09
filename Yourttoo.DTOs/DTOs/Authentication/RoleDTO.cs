using Yourttoo.DTOs.Common;

namespace Yourttoo.DTOs.DTOs.Authentication
{
    public class RoleDTO : IDataTransferObject
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public bool IsActive { get; set; }
    }
}
