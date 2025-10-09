using Yourttoo.DTOs.Shared.Features;

namespace Yourttoo.DTOs.Requests.FrequentlyAskedQuestion.Content
{
    public class BulkCreateFAQContentRequest : RequestPayloadBase
    {
        public List<CreateFAQContentRequest> Contents { get; set; } = new();
        
        public BulkCreateFAQContentRequest() { }
    }
}
