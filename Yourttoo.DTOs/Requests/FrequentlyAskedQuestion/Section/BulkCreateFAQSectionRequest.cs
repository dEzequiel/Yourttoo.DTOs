using Yourttoo.DTOs.Common;
using Yourttoo.DTOs.Shared.Features;

namespace Yourttoo.DTOs.Requests.FrequentlyAskedQuestion.Section
{
    public class BulkCreateFAQSectionRequest : RequestPayloadBase
    {
        public List<CreateFAQSectionRequest> Sections { get; set; } = new();
        
        public BulkCreateFAQSectionRequest() { }
    }
}
