using Yourttoo.DTOs.Shared.Features;

namespace Yourttoo.DTOs.Requests.AdditionalText
{
    public class QueryAdditionalTextRequest : QueryPayloadBase
    {
        public string? SearchTerm { get; set; } = null;
        public string? Status { get; set; } = null;
        public QueryAdditionalTextRequest() { }
    }
}
