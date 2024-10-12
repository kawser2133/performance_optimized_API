using AutoMapper;
using Performance_Optimized.Core.Entities.Business;
using Performance_Optimized.Core.Entities.General;
using Performance_Optimized.Core.Exceptions;
using Performance_Optimized.Core.Interfaces.IRepositories;
using Performance_Optimized.Core.Interfaces.IServices;
using System.Linq.Expressions;

namespace Performance_Optimized.Core.Services
{
    public class PurchaseService : IPurchaseService
    {
        private readonly IProductRepository _productRepository;
        private readonly IPurchaseRepository _purchaseRepository;
        private readonly IPurchaseDetailRepository _purchaseDetailRepository;
        private readonly IMapper _mapper;

        public PurchaseService(
            IProductRepository productRepository,
            IPurchaseRepository purchaseRepository,
            IPurchaseDetailRepository purchaseDetailRepository,
            IMapper mapper)
        {
            _productRepository = productRepository;
            _purchaseRepository = purchaseRepository;
            _purchaseDetailRepository = purchaseDetailRepository;
            _mapper = mapper;
        }

        public async Task<PaginatedDataViewModel<PurchaseViewModel>> Get(int pageNumber, int pageSize, string sortBy, string sortOrder, CancellationToken cancellationToken)
        {
            return await _purchaseRepository.GetPaginatedData(pageNumber, pageSize, sortBy, sortOrder, cancellationToken);
        }

        public async Task<PurchaseViewModel> GetById(int purchaseId, CancellationToken cancellationToken)
        {
            var purchase = await _purchaseRepository.GetById(purchaseId, cancellationToken);
            if (purchase == null)
            {
                throw new NotFoundException("Purchase not found");
            }
            return _mapper.Map<PurchaseViewModel>(purchase);
        }

        public async Task<IEnumerable<PurchaseViewModel>> GetPurchasesByCustomer(int customerId)
        {
            return await _purchaseRepository.GetPurchasesByCustomer(customerId);
        }

        public async Task<PurchaseViewModel> Create(PurchaseCreateViewModel model, CancellationToken cancellationToken)
        {
            var newPurchase = _mapper.Map<Purchase>(model);
            newPurchase.Created = DateTime.Now;

            decimal totalAmount = 0;

            foreach (var detail in newPurchase.PurchaseDetails)
            {
                var product = await _productRepository.GetById(detail.ProductId, cancellationToken);

                detail.Created = DateTime.Now;
                detail.Price = product.Price;
                detail.TotalAmount = Convert.ToDecimal(detail.Quantity * detail.Price);
                totalAmount += detail.TotalAmount;
            }

            newPurchase.TotalAmount = totalAmount;

            var purchase = await _purchaseRepository.Create(newPurchase, cancellationToken);
            return _mapper.Map<PurchaseViewModel>(purchase);
        }

        public async Task<PurchaseViewModel> Update(PurchaseUpdateViewModel model, CancellationToken cancellationToken)
        {
            var includePurchaseDetails = new List<Expression<Func<Purchase, object>>>
            {
                purchaseDetails => purchaseDetails.PurchaseDetails
            };

            var existingPurchase = await _purchaseRepository.GetById<int>(includePurchaseDetails, model.Id, cancellationToken);
            if (existingPurchase == null)
            {
                throw new NotFoundException("Purchase not found");
            }

            existingPurchase.PurchaseDate = model.PurchaseDate;
            existingPurchase.CustomerId = model.CustomerId;
            existingPurchase.Updated = DateTime.Now;

            decimal totalAmount = 0;

            var currentDetails = existingPurchase.PurchaseDetails.ToList();
            var updatedDetailIds = model.PurchaseDetails.Select(pd => pd.Id).ToList();

            // 1. Remove PurchaseDetails that are no longer in the update model
            var detailsToRemove = currentDetails.Where(cd => !updatedDetailIds.Contains(cd.Id)).ToList();
            foreach (var detail in detailsToRemove)
            {
                existingPurchase.PurchaseDetails.Remove(detail);
                _purchaseDetailRepository.RemoveItem(detail);
            }

            // 2. Update existing PurchaseDetails or add new ones
            foreach (var detailModel in model.PurchaseDetails)
            {
                var existingDetail = currentDetails.FirstOrDefault(cd => cd.Id == detailModel.Id);

                if (existingDetail != null)
                {
                    existingDetail.Quantity = detailModel.Quantity;

                    var product = await _productRepository.GetById(detailModel.ProductId, cancellationToken);
                    if (product != null)
                    {
                        existingDetail.Price = product.Price;
                    }
                    existingDetail.TotalAmount = Convert.ToDecimal(existingDetail.Quantity * existingDetail.Price);
                    existingDetail.Updated = DateTime.Now;
                }
                else
                {
                    var product = await _productRepository.GetById(detailModel.ProductId, cancellationToken);

                    var newDetail = _mapper.Map<PurchaseDetail>(detailModel);
                    if (product != null)
                    {
                        newDetail.Price = product.Price;
                    }
                    newDetail.TotalAmount = Convert.ToDecimal(newDetail.Quantity * newDetail.Price);
                    newDetail.Created = DateTime.Now;
                    existingPurchase.PurchaseDetails.Add(newDetail);
                }
            }

            // 3. Recalculate the total amount for the purchase by summing up all details' total amounts
            totalAmount = existingPurchase.PurchaseDetails.Sum(pd => pd.TotalAmount);
            existingPurchase.TotalAmount = totalAmount;

            var updatedPurchase = await _purchaseRepository.Update(existingPurchase, cancellationToken);
            return _mapper.Map<PurchaseViewModel>(updatedPurchase);
        }


        public async Task Delete(int purchaseId, CancellationToken cancellationToken)
        {
            var existingPurchase = await _purchaseRepository.GetById<int>(purchaseId, cancellationToken);
            if (existingPurchase == null)
            {
                throw new NotFoundException("Purchase not found");
            }

            await _purchaseRepository.Delete(existingPurchase, cancellationToken);
        }
    }

}
