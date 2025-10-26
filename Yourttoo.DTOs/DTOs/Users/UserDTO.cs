namespace Yourttoo.DTOs.DTOs.Users
{
    public class UserDTO : DataTransferObject
    {
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string Language { get; set; } = string.Empty;
        public string TimeZone { get; set; } = string.Empty;
        public string? AccountId { get; set; }
        public string? Avatar { get; set; }
        public string Status { get; set; } = string.Empty;
        public IList<string> Roles { get; set; } = new List<string>();
    }
}