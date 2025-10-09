using Yourttoo.DTOs.Shared.Features;

namespace Yourttoo.DTOs.Requests.FrequentlyAskedQuestion.Content
{
    public class DeleteFAQContentRequest : RequestPayloadBase
    {

        public Guid Id { get; set; } = Guid.Empty;
        public DeleteFAQContentRequest() { }
    }
}
