
namespace Yourttoo.DTOs.Requests.Authentication
{
    public class LoginRequest 
    {
        public string Email { get; set; } = string.Empty;
        public bool RememberMe { get; set; } = false;
        
    }
}
