using Yourttoo.DTOs.Common;
using Yourttoo.DTOs.Shared.Features;

namespace Yourttoo.DTOs.Requests.AdditionalText
{
    public class DeleteAdditionalTextRequest : RequestPayloadBase
    {
        public Guid Id { get; set; } = Guid.Empty;

        public DeleteAdditionalTextRequest() { }
    }
}