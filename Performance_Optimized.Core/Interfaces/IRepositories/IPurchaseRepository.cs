using Performance_Optimized.Core.Entities.Business;
using Performance_Optimized.Core.Entities.General;

namespace Performance_Optimized.Core.Interfaces.IRepositories
{
    public interface IPurchaseRepository : IBaseRepository<Purchase>
    {
        new Task<PaginatedDataViewModel<PurchaseViewModel>> GetPaginatedData(int pageNumber, int pageSize, string sortBy, string sortOrder, CancellationToken cancellationToken);
        Task<PurchaseViewModel> GetById(int purchaseId, CancellationToken cancellationToken);
        Task<IEnumerable<PurchaseViewModel>> GetPurchasesByCustomer(int customerId);
    }
}
