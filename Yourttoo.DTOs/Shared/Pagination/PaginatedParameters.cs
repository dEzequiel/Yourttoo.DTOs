namespace Yourttoo.DTOs.Shared.Pagination
{
    public class PaginatedParameters
    {
        public int Page { get; set; }

        /// <summary>
        /// Page size
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// Total number of records
        /// </summary>
        public int TotalCount { get; set; }

        /// <summary>
        /// Total number of pages
        /// </summary>
        public int TotalPages { get; set; }

        /// <summary>
        /// Indicates if there is a next page
        /// </summary>
        public bool HasNextPage { get; set; }

        /// <summary>
        /// Indicates if there is a previous page
        /// </summary>
        public bool HasPreviousPage { get; set; }

        /// <summary>
        /// Default constructor
        /// </summary>
        public PaginatedParameters() { }

        /// <summary>
        /// Constructor with parameters
        /// </summary>
        public PaginatedParameters(int page, int pageSize, int totalCount)
        {
            Page = page;
            PageSize = pageSize;
            TotalCount = totalCount;
            TotalPages = (int)Math.Ceiling((double)totalCount / pageSize);
            HasNextPage = page * pageSize < totalCount;
            HasPreviousPage = page > 1;
        }
    }
}

