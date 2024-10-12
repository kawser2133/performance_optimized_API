using Bogus;
using Performance_Optimized.Core.Entities.General;

namespace Performance_Optimized.Infrastructure.Data
{
    public static class DataSeeder
    {
        public static async Task SeedData(ApplicationDbContext context)
        {
            // Ensure the database is created
            await context.Database.EnsureCreatedAsync();

            // Check if there are already data present
            if (context.Products.Any() || context.Customers.Any() || context.Purchases.Any())
            {
                return; // DB has been seeded
            }

            // Create a list of products
            var products = new Faker<Product>()
                .RuleFor(p => p.Name, f => f.Commerce.ProductName())
                .RuleFor(p => p.Code, f => f.Commerce.Random.String2(5).ToUpper())
                .RuleFor(p => p.Price, f => (float)Math.Round(f.Finance.Amount(1, 100), 2))
                .RuleFor(p => p.Created, f => f.Date.Recent())
                .RuleFor(p => p.IsActive, true);

            var productList = products.Generate(500);

            // Add products to the context
            await context.Products.AddRangeAsync(productList);

            // Create a list of customers
            var customers = new Faker<Customer>()
                .RuleFor(c => c.FirstName, f => f.Name.FirstName())
                .RuleFor(c => c.LastName, f => f.Name.LastName())
                .RuleFor(c => c.Email, f => f.Internet.Email())
                .RuleFor(c => c.PhoneNumber, f => f.Phone.PhoneNumber())
                .RuleFor(c => c.Created, f => f.Date.Recent());

            var customerList = customers.Generate(5000);

            // Add customers to the context
            await context.Customers.AddRangeAsync(customerList);

            // Create a list of purchases
            var purchaseFaker = new Faker<Purchase>()
                .RuleFor(p => p.PurchaseDate, f => f.Date.Recent())
                .RuleFor(p => p.Customer, f => f.PickRandom(customerList))
                .RuleFor(p => p.Created, f => f.Date.Recent())

                .RuleFor(p => p.PurchaseDetails, f =>
                {
                    // Generate random PurchaseDetails for each Purchase
                    var details = new Faker<PurchaseDetail>()
                    .RuleFor(pd => pd.Quantity, f => f.Random.Int(1, 5))
                    .RuleFor(pd => pd.Price, f => (float)Math.Round(f.Finance.Amount(1, 100), 2))
                    .RuleFor(pd => pd.Product, f => f.PickRandom(productList))
                    .RuleFor(pd => pd.TotalAmount, (f, pd) => Convert.ToDecimal(pd.Quantity * pd.Price))
                    .RuleFor(pd => pd.Created, f => f.Date.Recent())

                    .Generate(f.Random.Int(1, 7));

                    return new List<PurchaseDetail>(details);
                })
                .RuleFor(p => p.TotalAmount, (f, p) => p.PurchaseDetails.Sum(pd => pd.TotalAmount));

            // Generate a list of purchases
            var purchaseList = purchaseFaker.Generate(20000);


            // Add purchases to the context
            await context.Purchases.AddRangeAsync(purchaseList);

            // Save all changes to the database
            await context.SaveChangesAsync();
        }
    }
}
