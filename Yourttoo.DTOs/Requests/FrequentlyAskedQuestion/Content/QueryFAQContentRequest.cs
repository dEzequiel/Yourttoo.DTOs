using Yourttoo.DTOs.Shared.Features;

namespace Yourttoo.DTOs.Requests.FrequentlyAskedQuestion.Content
{
    public class QueryFAQContentRequest : QueryPayloadBase
    {
        public string? SearchTerm { get; set; }
        public string? Status { get; set; }
        public string? Slug { get; set; }
        public Guid? FAQSectionId { get; set; }
        
        public QueryFAQContentRequest() { }
    }
}
