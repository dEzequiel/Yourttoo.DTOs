
namespace Yourttoo.DTOs.Requests.Authentication
{
    public class CreateLoginRequest 
    {
        public string Email { get; set; } = string.Empty;
        public bool RememberMe { get; set; } = false;
        
    }
}
