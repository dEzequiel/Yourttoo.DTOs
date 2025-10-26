namespace Yourttoo.DTOs.DTOs.Users
{
    public class AccountDTO : DataTransferObject
    {
        public string Email { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public string AccountType { get; set; } = string.Empty;
    }
}