using Yourttoo.DTOs.Shared.Pagination;

namespace Yourttoo.DTOs.Shared.Features
{
    /// <summary>
    /// Base class for payloads of a Query
    /// Contains common properties that all queries should have
    /// </summary>
    public abstract class QueryPayloadBase : RequestPayloadBase
    {
        public PaginatedParameters? Pagination { get; set; } 
    }
}
