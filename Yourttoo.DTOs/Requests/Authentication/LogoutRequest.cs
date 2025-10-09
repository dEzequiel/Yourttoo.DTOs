using Yourttoo.DTOs.Shared.Features;

namespace Yourttoo.DTOs.Requests.Authentication
{
    public class LogoutRequest
    {
        public string SessionId { get; set; } = string.Empty;
        
        public LogoutRequest() { }
    }
}
