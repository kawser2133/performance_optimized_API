using Performance_Optimized.Core.Interfaces.IRepositories;
using Performance_Optimized.Core.Interfaces.IServices;
using Performance_Optimized.Core.Services;
using Performance_Optimized.Infrastructure.Repositories;

namespace Performance_Optimized.API.Extensions
{
    public static class ServiceExtension
    {
        public static IServiceCollection RegisterService(this IServiceCollection services)
        {
            #region Services

            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<ICustomerService, CustomerService>();
            services.AddScoped<IPurchaseService, PurchaseService>();
            #endregion


            #region Repositories

            services.AddTransient<IProductRepository, ProductRepository>();
            services.AddTransient<ICustomerRepository, CustomerRepository>();
            services.AddTransient<IPurchaseRepository, PurchaseRepository>();
            services.AddTransient<IPurchaseDetailRepository, PurchaseDetailRepository>();
            #endregion

            return services;
        }

    }
}
