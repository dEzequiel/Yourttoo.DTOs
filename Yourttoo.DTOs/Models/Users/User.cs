using Yourttoo.DTOs.Common.Constants;

namespace Yourttoo.DTOs.Models.Users
{
    public class User : IEntity
    {
        public string? Username { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? FullName => $"{FirstName} {LastName}";
        public string Language { get; set; } = Languages.Default; // Default to Spanish
        public string TimeZone { get; set; } = TimeZones.Europe.MADRID; // Default to UTC

        public string? AccountId { get; set; }

        public string? Email { get; set; }
        public string? PasswordHash { get; set; }
        public string? PasswordSalt { get; set; }

        public string? AccessToken { get; set; }
        public DateTime? AccessTokenExpiry { get; set; }

        public DateTime? LastLoginAt { get; set; }

        public int FailedLoginAttempts { get; set; }
        public string Status { get; set; } = UserStatus.ACTIVE; // Default to activefPformat

        public IList<string> Roles { get; set; } = new List<string>() { RoleNames.ADMIN }; // Use Role constants

    }
}