using Yourttoo.DTOs.Shared.Features;

namespace Yourttoo.DTOs.Requests.Account
{
    public class DeleteAccountRequest : RequestPayloadBase
    {
        public string Email { get; set; } = string.Empty;
    }
}
