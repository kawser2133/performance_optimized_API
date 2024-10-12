
using Performance_Optimized.Core.Entities.Business;
using System.Linq.Expressions;

namespace Performance_Optimized.Core.Interfaces.IRepositories
{
    public interface IBaseRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAll(CancellationToken cancellationToken);
        Task<PaginatedDataViewModel<T>> GetPaginatedData(int pageNumber, int pageSize, string sortBy, string sortOrder, CancellationToken cancellationToken);
        Task<T> GetById<TType>(TType id, CancellationToken cancellationToken);
        Task<T> GetById<Tid>(List<Expression<Func<T, object>>> includeExpressions, Tid id, CancellationToken cancellationToken);
        Task<T> Create(T model, CancellationToken cancellationToken);
        Task<T> Update(T model, CancellationToken cancellationToken);
        Task Delete(T model, CancellationToken cancellationToken);
        void RemoveItem(T model);
        Task SaveChanges(CancellationToken cancellationToken);
    }
}
