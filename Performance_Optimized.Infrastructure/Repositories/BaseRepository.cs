using Performance_Optimized.Core.Entities.Business;
using Performance_Optimized.Core.Exceptions;
using Performance_Optimized.Core.Interfaces.IRepositories;
using Performance_Optimized.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Performance_Optimized.Infrastructure.Repositories
{
    public class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        protected readonly ApplicationDbContext _dbContext;

        public BaseRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<T>> GetAll(CancellationToken cancellationToken = default)
        {
            var result = await _dbContext.Set<T>()
                .AsNoTracking()
                .ToListAsync(cancellationToken);

            return result;
        }

        public async Task<PaginatedDataViewModel<T>> GetPaginatedData(int pageNumber, int pageSize, string sortBy, string sortOrder, CancellationToken cancellationToken = default)
        {
            var query = _dbContext.Set<T>().AsNoTracking();

            // Add sorting
            if (!string.IsNullOrEmpty(sortBy))
            {
                var orderByExpression = GetOrderByExpression<T>(sortBy);
                query = sortOrder?.ToLower() == "desc" ? query.OrderByDescending(orderByExpression) : query.OrderBy(orderByExpression);
            }

            // Pagination
            var data = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);

            var totalCount = await query.CountAsync(cancellationToken);

            return new PaginatedDataViewModel<T>(data, totalCount);
        }

        public Expression<Func<T, object>> GetOrderByExpression<T>(string propertyName)
        {
            var parameter = Expression.Parameter(typeof(T), "x");
            var property = Expression.Property(parameter, propertyName);
            var conversion = Expression.Convert(property, typeof(object));

            return Expression.Lambda<Func<T, object>>(conversion, parameter);
        }

        public async Task<T> GetById<TType>(TType id, CancellationToken cancellationToken = default)
        {
            var result = await _dbContext.Set<T>().FindAsync(id, cancellationToken);

            if (result == null)
            {
                throw new NotFoundException("No data found");
            }

            return result;
        }

        public async Task<T> GetById<Tid>(List<Expression<Func<T, object>>> includeExpressions, Tid id, CancellationToken cancellationToken = default)
        {
            var query = _dbContext.Set<T>().AsQueryable();

            // Include navigation properties if provided
            if (includeExpressions != null)
            {
                query = includeExpressions.Aggregate(query, (current, include) => current.Include(include));
            }

            // Use reflection to get the key property dynamically
            var keyProperty = _dbContext.Model.FindEntityType(typeof(T))
                                        .FindPrimaryKey()
                                        .Properties
                                        .FirstOrDefault();

            if (keyProperty == null)
            {
                throw new InvalidOperationException("No key property found for the entity.");
            }

            // Get the key property's name
            var keyPropertyName = keyProperty.Name;

            // Build the query with a dynamic key comparison using EF.Property
            var data = await query.SingleOrDefaultAsync(x => EF.Property<Tid>(x, keyPropertyName).Equals(id), cancellationToken);

            if (data == null)
            {
                throw new NotFoundException("No data found");
            }

            return data;
        }


        public async Task<T> Create(T model, CancellationToken cancellationToken = default)
        {
            await _dbContext.Set<T>().AddAsync(model, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return model;
        }

        public async Task<T> Update(T model, CancellationToken cancellationToken = default)
        {
            _dbContext.Set<T>().Update(model);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return model;
        }

        public async Task Delete(T model, CancellationToken cancellationToken = default)
        {
            _dbContext.Set<T>().Remove(model);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public void RemoveItem(T model)
        {
            _dbContext.Set<T>().Remove(model);
        }

        public async Task SaveChanges(CancellationToken cancellationToken = default)
        {
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
