using Yourttoo.DTOs.Shared.Features;

namespace Yourttoo.DTOs.Requests.FrequentlyAskedQuestion.Section
{
    public class DeleteFAQSectionRequest : RequestPayloadBase
    {
        public Guid Id { get; set; } = Guid.Empty;
        public DeleteFAQSectionRequest() { }
    }
}
