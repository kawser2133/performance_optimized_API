using Performance_Optimized.Core.Entities.Business;
using Performance_Optimized.Core.Entities.General;

namespace Performance_Optimized.Core.Interfaces.IRepositories
{
    public interface ICustomerRepository : IBaseRepository<Customer>
    {
        Task<CustomerViewModel> GetCustomerWithPurchases(int customerId, CancellationToken cancellationToken);
    }
}
