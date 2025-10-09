using Yourttoo.DTOs.Shared.Features;

namespace Yourttoo.DTOs.Requests.FrequentlyAskedQuestion.Section
{
    public class QueryFAQSectionRequest : QueryPayloadBase
    {
        public string? SearchTerm { get; set; }
        public string? Category { get; set; }
        public string? Type { get; set; }
        
        public QueryFAQSectionRequest() { }
    }
}
