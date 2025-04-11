using System.Linq.Expressions;
using TestApi.Domain.Entities;
using TestApi.Domain.Models.Paginations;

namespace TestApi.Infrastructure.Repositories
{
    public interface IBaseRepository<T> where T : BaseEntity
    {
        Task<T> GetByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<IList<T>> GetByConditionAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default);
        Task<PaginatedModel<List<T>>> GetAllPaginatedAsync(int pageIndex = 0, int pageSize = 10, CancellationToken cancellationToken = default);
        Task<bool> AddAsync(T entity, CancellationToken cancellationToken = default);
        bool Update(T entity);
        Task<bool> HardRemoveAsync(T entity, CancellationToken cancellationToken = default);
        bool SoftRemove(T entity);

    }
}
