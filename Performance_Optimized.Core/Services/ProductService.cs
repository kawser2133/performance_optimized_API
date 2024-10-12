using AutoMapper;
using Performance_Optimized.Core.Entities.Business;
using Performance_Optimized.Core.Entities.General;
using Performance_Optimized.Core.Exceptions;
using Performance_Optimized.Core.Interfaces.IRepositories;
using Performance_Optimized.Core.Interfaces.IServices;

namespace Performance_Optimized.Core.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;

        public ProductService(IProductRepository productRepository, IMapper mapper)
        {
            _productRepository = productRepository;
            _mapper = mapper;
        }

        public async Task<PaginatedDataViewModel<ProductViewModel>> Get(int pageNumber, int pageSize, string sortBy, string sortOrder, CancellationToken cancellationToken)
        {
            var products = await _productRepository.GetPaginatedData(pageNumber, pageSize, sortBy, sortOrder, cancellationToken);
            return _mapper.Map<PaginatedDataViewModel<ProductViewModel>>(products);
        }

        public async Task<ProductViewModel> GetById(int id, CancellationToken cancellationToken)
        {
            var product = await _productRepository.GetById(id, cancellationToken);
            if (product == null)
            {
                throw new NotFoundException("Product not found");
            }
            return _mapper.Map<ProductViewModel>(product);
        }

        public async Task<ProductViewModel> Create(ProductCreateViewModel model, CancellationToken cancellationToken)
        {
            var newProduct = _mapper.Map<Product>(model);
            newProduct.Created = DateTime.Now;

            var product = await _productRepository.Create(newProduct, cancellationToken);
            return _mapper.Map<ProductViewModel>(product);
        }

        public async Task<ProductViewModel> Update(ProductUpdateViewModel model, CancellationToken cancellationToken)
        {
            var existingProduct = await _productRepository.GetById(model.Id, cancellationToken);
            if (existingProduct == null)
            {
                throw new NotFoundException("Product not found");
            }

            _mapper.Map(model, existingProduct);
            existingProduct.Updated = DateTime.Now;

            var updatedProduct = await _productRepository.Update(existingProduct, cancellationToken);
            return _mapper.Map<ProductViewModel>(updatedProduct);
        }

        public async Task Delete(int id, CancellationToken cancellationToken)
        {
            var existingProduct = await _productRepository.GetById(id, cancellationToken);
            if (existingProduct == null)
            {
                throw new NotFoundException("Product not found");
            }

            await _productRepository.Delete(existingProduct, cancellationToken);
        }
    }

}
