using Yourttoo.DTOs.Common.Constants;

namespace Yourttoo.DTOs.Models.Users
{
    public class Account
    {
       public string? Email { get; set; }
        public string Status { get; set; } = UserStatus.ACTIVE; // e.g., active, inactive, pending, suspended (use UserStatus constants)
        public DateTime? LastLoginAt { get; set; }
        public int FailedLoginAttempts { get; set; }
        public string? PasswordHash { get; set; }
        public string? PasswordSalt { get; set; }
        public string? PasswordChangedAt { get; set; }
        public bool IsTwoFactorEnabled { get; set; } = false;
        public string? TwoFactorEnabledAt { get; set; }
        public string? TwoFactorMethod { get; set; } // e.g., SMS, Authenticator App
        public string? LastPasswordResetAt { get; set; }
        public string? PasswordResetToken { get; set; }
        public string? PasswordResetTokenExpiresAt { get; set; }
        public string AccountType { get; set; } = AccountTypes.AGENCY; // e.g.,  (use AccountType constants)
        public IList<string> ApiKeys { get; set; } = new List<string>(); // List of API keys associated with the account

    }
}