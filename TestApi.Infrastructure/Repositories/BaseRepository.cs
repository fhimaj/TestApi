using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using TestApi.Domain.Entities;
using TestApi.Domain.Models.Paginations;
using TestApi.Infrastructure.DbContexts;
using TestApi.Infrastructure.Extensions;

namespace TestApi.Infrastructure.Repositories
{
    public class BaseRepository<T> : IBaseRepository<T> where T : BaseEntity
    {
        private readonly MainDbContext _dbContext;
        private readonly DbSet<T> _dbSet;

        public BaseRepository(MainDbContext dbContext)
        {
            _dbContext = dbContext;
            _dbSet = dbContext.Set<T>();
        }

        public async Task<T> GetByIdAsync(
            int id,
            CancellationToken cancellationToken = default)
        {
            if (id == 0)
                return null;

            return await _dbSet.OrderByDescending(x => x.InsertDate)
                                .FirstOrDefaultAsync(x => x.Id == id && !x.Deleted, cancellationToken);
        }

        public async Task<IList<T>> GetByConditionAsync(
            Expression<Func<T, bool>> predicate,
            CancellationToken cancellationToken = default)
        {
            if (predicate == null)
                return default;

            Expression<Func<T, bool>> deletedCheck = x => x.Deleted == false;

            predicate = predicate.And(deletedCheck);

            return await _dbSet.Where(predicate)
                .OrderByDescending(x => x.InsertDate)
                .ToListAsync(cancellationToken);
        }

        public async Task<PaginatedModel<List<T>>> GetAllPaginatedAsync(
            int pageIndex = 0,
            int pageSize = 10,
            CancellationToken cancellationToken = default)
        {
            var query = _dbSet.Where(x => !x.Deleted);

            var totalCount = await query.CountAsync(cancellationToken);

            if (pageIndex <= 0)
                pageIndex = 1;

            if (pageSize <= 0)
                pageSize = 10;

            var items = await query.OrderByDescending(x => x.InsertDate)
                                   .Skip((pageIndex - 1) * pageSize)
                                   .Take(pageSize)
                                   .ToListAsync(cancellationToken);

            var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

            return new PaginatedModel<List<T>>
            {
                Model = items,
                PageIndex = pageIndex,
                PageSize = pageSize,
                TotalCount = totalCount,
                TotalPages = totalPages,
                PageCount = items.Count,
            };
        }

        public async Task<bool> AddAsync(
            T entity,
            CancellationToken cancellationToken = default)
        {
            var addedEntity = await _dbSet.AddAsync(entity, cancellationToken);
            return await _dbContext.SaveChangesAsync(cancellationToken) > 0;
        }

        public bool Update(T entity)
        {
            if (entity == null)
                return false;

            _dbSet.Update(entity);
            return _dbContext.SaveChanges() > 0;
        }

        public bool SoftRemove(T entity)
        {
            if (entity == null)
                return false;

            entity.Deleted = true;

            _dbSet.Update(entity);
            return _dbContext.SaveChanges() > 0;
        }

        public async Task<bool> HardRemoveAsync(
            T entity,
            CancellationToken cancellationToken = default)
        {
            if (entity == null)
                return false;

            _dbSet.Remove(entity);
            return await _dbContext.SaveChangesAsync(cancellationToken) > 0;
        }
    }
}
