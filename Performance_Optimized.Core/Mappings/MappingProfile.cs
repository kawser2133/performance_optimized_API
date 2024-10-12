using AutoMapper;
using Performance_Optimized.Core.Entities.Business;
using Performance_Optimized.Core.Entities.General;

namespace Performance_Optimized.Core.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Product
            CreateMap<Product, ProductViewModel>().ReverseMap();
            CreateMap<Product, ProductCreateViewModel>().ReverseMap();
            CreateMap<Product, ProductUpdateViewModel>().ReverseMap();
            CreateMap<PaginatedDataViewModel<Product>, PaginatedDataViewModel<ProductViewModel>>();

            // Customer
            CreateMap<Customer, CustomerViewModel>().ReverseMap();
            CreateMap<Customer, CustomerCreateViewModel>().ReverseMap();
            CreateMap<Customer, CustomerUpdateViewModel>().ReverseMap();
            CreateMap<PaginatedDataViewModel<Customer>, PaginatedDataViewModel<CustomerViewModel>>();

            // Purchase
            CreateMap<Purchase, PurchaseViewModel>().ReverseMap();
            CreateMap<Purchase, PurchaseCreateViewModel>().ReverseMap();
            CreateMap<Purchase, PurchaseUpdateViewModel>().ReverseMap();
            CreateMap<PaginatedDataViewModel<Purchase>, PaginatedDataViewModel<PurchaseViewModel>>();

            // Purchase Details
            CreateMap<PurchaseDetail, PurchaseDetailViewModel>().ReverseMap();
            CreateMap<PurchaseDetail, PurchaseDetailCreateViewModel>().ReverseMap();
            CreateMap<PurchaseDetail, PurchaseDetailUpdateViewModel>().ReverseMap();


        }
    }
}
