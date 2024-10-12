using Performance_Optimized.Core.Entities.Business;
using Performance_Optimized.Core.Entities.General;
using Performance_Optimized.Core.Exceptions;
using Performance_Optimized.Core.Interfaces.IRepositories;
using Performance_Optimized.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Performance_Optimized.Infrastructure.Repositories
{
    public class CustomerRepository : BaseRepository<Customer>, ICustomerRepository
    {
        public CustomerRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<CustomerViewModel> GetCustomerWithPurchases(int customerId, CancellationToken cancellationToken)
        {
            // AsNoTracking improves performance since we don't need to track changes
            var customer = await _dbContext.Customers
                .Where(c => c.Id == customerId)
                .Select(c => new CustomerViewModel
                {
                    Id = c.Id,
                    FirstName = c.FirstName,
                    LastName = c.LastName,
                    Email = c.Email,
                    PhoneNumber = c.PhoneNumber,
                    Created = c.Created,
                    Updated = c.Updated,
                    Purchases = c.Purchases.Select(p => new PurchaseViewModel
                    {
                        Id = p.Id,
                        PurchaseDate = p.PurchaseDate,
                        TotalAmount = p.TotalAmount,
                        CustomerId = p.CustomerId,
                        CustomerName = $"{p.Customer.FirstName} {p.Customer.LastName}",
                        PurchaseDetails = p.PurchaseDetails.Select(pd => new PurchaseDetailViewModel
                        {
                            Id = pd.Id,
                            Quantity = pd.Quantity,
                            Price = pd.Price,
                            TotalAmount = pd.TotalAmount,
                            ProductId = pd.ProductId,
                            ProductName = pd.Product.Name,
                            Created = pd.Created,
                            Updated = pd.Updated
                        }).ToList(),
                        Created = p.Created,
                        Updated = p.Updated
                    }).ToList()
                })
                .AsNoTracking()
                .FirstOrDefaultAsync(cancellationToken);


            if (customer == null)
            {
                throw new NotFoundException("Customer not found");
            }

            return customer;
        }



    }
}
