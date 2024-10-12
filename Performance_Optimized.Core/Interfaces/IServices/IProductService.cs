using Performance_Optimized.Core.Entities.Business;

namespace Performance_Optimized.Core.Interfaces.IServices
{
    public interface IProductService
    {
        Task<PaginatedDataViewModel<ProductViewModel>> Get(int pageNumber, int pageSize, string sortBy, string sortOrder, CancellationToken cancellationToken);
        Task<ProductViewModel> GetById(int id, CancellationToken cancellationToken);
        Task<ProductViewModel> Create(ProductCreateViewModel model, CancellationToken cancellationToken);
        Task<ProductViewModel> Update(ProductUpdateViewModel model, CancellationToken cancellationToken);
        Task Delete(int id, CancellationToken cancellationToken);
    }
}
