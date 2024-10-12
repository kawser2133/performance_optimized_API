using Dapper;
using Performance_Optimized.Core.Entities.Business;
using Performance_Optimized.Core.Entities.General;
using Performance_Optimized.Core.Exceptions;
using Performance_Optimized.Core.Interfaces.IRepositories;
using Performance_Optimized.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Performance_Optimized.Infrastructure.Repositories
{
    public class PurchaseRepository : BaseRepository<Purchase>, IPurchaseRepository
    {
        public PurchaseRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }

        public new async Task<PaginatedDataViewModel<PurchaseViewModel>> GetPaginatedData(int pageNumber, int pageSize, string sortBy, string sortOrder, CancellationToken cancellationToken = default)
        {
            var query = _dbContext.Purchases
                .AsNoTracking();

            // Add sorting
            if (!string.IsNullOrEmpty(sortBy))
            {
                var orderByExpression = GetOrderByExpression<Purchase>(sortBy);
                query = sortOrder?.ToLower() == "desc" ? query.OrderByDescending(orderByExpression) : query.OrderBy(orderByExpression);
            }

            var totalCount = await query.CountAsync(cancellationToken);

            var purchases = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(p => new PurchaseViewModel
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
                })
                .ToListAsync(cancellationToken);

            return new PaginatedDataViewModel<PurchaseViewModel>(purchases, totalCount);
        }

        public async Task<PurchaseViewModel> GetById(int purchaseId, CancellationToken cancellationToken = default)
        {
            var purchases = await _dbContext.Purchases
                .Where(p => p.Id == purchaseId)
                .Select(p => new PurchaseViewModel
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
                })
                .FirstOrDefaultAsync(cancellationToken);

            if (purchases == null)
            {
                throw new NotFoundException("No data found");
            }

            return purchases;
        }

        public async Task<IEnumerable<PurchaseViewModel>> GetPurchasesByCustomer(int customerId)
        {
            // SQL query to fetch the data

            var sql = @"
                SELECT p.""Id"", p.""PurchaseDate"", p.""TotalAmount"",
                       p.""CustomerId"", c.""FirstName"", c.""LastName"", c.""Email"", c.""PhoneNumber"",
                       pd.""Id"" AS ""PurchaseDetailId"", pd.""ProductId"", pd.""Quantity"", pd.""Price"", 
                       pr.""Name"" AS ""ProductName""
                FROM ""Purchases"" p
                JOIN ""Customers"" c ON p.""CustomerId"" = c.""Id""
                LEFT JOIN ""PurchaseDetails"" pd ON pd.""PurchaseId"" = p.""Id""
                LEFT JOIN ""Products"" pr ON pd.""ProductId"" = pr.""Id""
                WHERE p.""CustomerId"" = @CustomerId;
                ";

            // Retrieve the underlying database connection from DbContext
            var connection = _dbContext.Database.GetDbConnection();

            // Open the connection if it's not already open
            if (connection.State == System.Data.ConnectionState.Closed)
            {
                await connection.OpenAsync();
            }

            try
            {
                var purchaseDict = new Dictionary<int, PurchaseViewModel>();

                // Use Dapper's async query method
                var result = await connection.QueryAsync<PurchaseViewModel, CustomerViewModel, PurchaseDetailViewModel, PurchaseViewModel>(
                    sql,
                    (purchase, customer, purchaseDetail) =>
                    {
                        // Find or create the purchase entry
                        if (!purchaseDict.TryGetValue(purchase.Id, out var currentPurchase))
                        {
                            currentPurchase = purchase;
                            currentPurchase.Customer = customer;
                            currentPurchase.PurchaseDetails = new List<PurchaseDetailViewModel>();
                            purchaseDict.Add(currentPurchase.Id, currentPurchase);
                        }

                        // Add purchase details
                        if (purchaseDetail != null)
                        {
                            currentPurchase.PurchaseDetails.Add(purchaseDetail);
                        }

                        return currentPurchase;
                    },
                    new { CustomerId = customerId }, // Parameter for customerId
                    splitOn: "CustomerId,PurchaseDetailId" // Split the result sets on these columns
                );

                return purchaseDict.Values;
            }
            finally
            {
                // Optionally, you can close the connection here if you're not relying on the DbContext to manage it.
                await connection.CloseAsync();
            }
        }

    }
}
