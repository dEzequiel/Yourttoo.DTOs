using System;

namespace Yourttoo.DTOs.DTOs.Authentication
{
    public class LoginDTO
    {
        public Guid UserId { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Language { get; set; } = string.Empty;
        public IList<string> Roles { get; set; } = new List<string>();

    }
}
