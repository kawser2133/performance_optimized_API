using Performance_Optimized.Core.Entities.Business;

namespace Performance_Optimized.Core.Interfaces.IServices
{
    public interface IPurchaseService
    {
        Task<PaginatedDataViewModel<PurchaseViewModel>> Get(int pageNumber, int pageSize, string sortBy, string sortOrder, CancellationToken cancellationToken);
        Task<PurchaseViewModel> GetById(int purchaseId, CancellationToken cancellationToken);
        Task<IEnumerable<PurchaseViewModel>> GetPurchasesByCustomer(int customerId);
        Task<PurchaseViewModel> Create(PurchaseCreateViewModel model, CancellationToken cancellationToken);
        Task<PurchaseViewModel> Update(PurchaseUpdateViewModel model, CancellationToken cancellationToken);
        Task Delete(int purchaseId, CancellationToken cancellationToken);
    }

}
