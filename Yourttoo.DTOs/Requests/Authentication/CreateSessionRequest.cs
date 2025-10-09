using Yourttoo.DTOs.Shared.Features;

namespace Yourttoo.DTOs.Requests.Authentication
{
    public class CreateSessionRequest
    {
        public string Email { get; set; } = string.Empty;
        public bool RememberMe { get; set; } = false;
        
        public CreateSessionRequest() { }
    }
}
