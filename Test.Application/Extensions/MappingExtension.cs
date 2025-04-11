using TestApi.Domain.Models.Paginations;

namespace TestApi.Application.Extensions
{
    public static class MappingExtension
    {
        public static PaginatedModel<List<T2>> ToPaginatedListResponseModel<T1, T2>(this PaginatedModel<List<T1>> entities, List<T2> models)
            where T1 : class
            where T2 : class
        {
            return new PaginatedModel<List<T2>>
            {
                Model = models,
                PageIndex = entities.PageIndex,
                PageSize = entities.PageSize,
                TotalCount = entities.TotalCount,
                PageCount = entities.PageCount,
                TotalPages = entities.TotalPages,
            };
        }
    }
}
