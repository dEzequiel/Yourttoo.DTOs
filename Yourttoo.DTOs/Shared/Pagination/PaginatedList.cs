namespace Yourttoo.DTOs.Shared.Pagination
{
    public class PaginatedList<T>
    {
        public PaginatedParameters Pagination { get; set; }
        public List<T> Items { get; set; }
        
        public PaginatedList(List<T> items, int count, int pageNumber, int pageSize)
        {
            Pagination = new PaginatedParameters
            {
                TotalCount = count,
                PageSize = pageSize,
                Page = pageNumber,
                TotalPages = (int)Math.Ceiling(count / (double)pageSize),
                HasNextPage = pageNumber * pageSize < count,
                HasPreviousPage = pageNumber > 1
            };
            Items = items;
        }

        public static PaginatedList<T> ToPaginatedList(IEnumerable<T> source, int pageNumber, int pageSize)
        {
            var count = source.Count();
            var items = source
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize).ToList();
            return new PaginatedList<T>(items, count, pageNumber, pageSize);
        }
    }
}
