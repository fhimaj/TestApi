namespace TestApi.Domain.Models.Paginations
{
    public class PaginatedModel<T> where T : class
    {
        public T Model { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; } = 10;
        public int PageCount { get; set; }
        public int TotalCount { get; set; }
        public int TotalPages { get; set; }
    }
}
