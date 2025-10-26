namespace Yourttoo.DTOs.DTOs.Users
{
    public class UserDTO : DataTransferObject
    {
        public string Username { get; set; } = string.Empty;
        public string? AccountId { get; set; }
        public IList<string> Roles { get; set; } = new List<string>();
    }
}