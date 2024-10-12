using Performance_Optimized.Core.Entities.Business;

namespace Performance_Optimized.Core.Interfaces.IServices
{
    public interface ICustomerService
    {
        Task<PaginatedDataViewModel<CustomerViewModel>> Get(int pageNumber, int pageSize, string sortBy, string sortOrder, CancellationToken cancellationToken);
        Task<CustomerViewModel> GetById(int id, CancellationToken cancellationToken);
        Task<CustomerViewModel> Create(CustomerCreateViewModel model, CancellationToken cancellationToken);
        Task<CustomerViewModel> Update(CustomerUpdateViewModel model, CancellationToken cancellationToken);
        Task Delete(int customerId, CancellationToken cancellationToken);
        Task<CustomerViewModel> GetCustomerWithPurchases(int customerId, CancellationToken cancellationToken);
    }
}
