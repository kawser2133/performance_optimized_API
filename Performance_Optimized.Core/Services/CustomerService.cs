using AutoMapper;
using Performance_Optimized.Core.Entities.Business;
using Performance_Optimized.Core.Entities.General;
using Performance_Optimized.Core.Exceptions;
using Performance_Optimized.Core.Interfaces.IRepositories;
using Performance_Optimized.Core.Interfaces.IServices;

namespace Performance_Optimized.Core.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IMapper _mapper;

        public CustomerService(ICustomerRepository customerRepository, IMapper mapper)
        {
            _customerRepository = customerRepository;
            _mapper = mapper;
        }

        public async Task<PaginatedDataViewModel<CustomerViewModel>> Get(int pageNumber, int pageSize, string sortBy, string sortOrder, CancellationToken cancellationToken)
        {
            var customers = await _customerRepository.GetPaginatedData(pageNumber, pageSize, sortBy, sortOrder, cancellationToken);
            return _mapper.Map<PaginatedDataViewModel<CustomerViewModel>>(customers);
        }

        public async Task<CustomerViewModel> GetById(int customerId, CancellationToken cancellationToken)
        {
            var customer = await _customerRepository.GetById(customerId, cancellationToken);
            if (customer == null)
            {
                throw new NotFoundException("Customer not found");
            }
            return _mapper.Map<CustomerViewModel>(customer);
        }

        public async Task<CustomerViewModel> Create(CustomerCreateViewModel model, CancellationToken cancellationToken)
        {
            var newCustomer = _mapper.Map<Customer>(model);
            newCustomer.Created = DateTime.Now;

            var customer = await _customerRepository.Create(newCustomer, cancellationToken);
            return _mapper.Map<CustomerViewModel>(customer);
        }

        public async Task<CustomerViewModel> Update(CustomerUpdateViewModel model, CancellationToken cancellationToken)
        {
            var existingCustomer = await _customerRepository.GetById(model.Id, cancellationToken);
            if (existingCustomer == null)
            {
                throw new NotFoundException("Customer not found");
            }

            _mapper.Map(model, existingCustomer);
            existingCustomer.Updated = DateTime.Now;

            var updatedCustomer = await _customerRepository.Update(existingCustomer, cancellationToken);
            return _mapper.Map<CustomerViewModel>(updatedCustomer);
        }

        public async Task Delete(int customerId, CancellationToken cancellationToken)
        {
            var existingCustomer = await _customerRepository.GetById(customerId, cancellationToken);
            if (existingCustomer == null)
            {
                throw new NotFoundException("Customer not found");
            }

            await _customerRepository.Delete(existingCustomer, cancellationToken);
        }

        public async Task<CustomerViewModel> GetCustomerWithPurchases(int customerId, CancellationToken cancellationToken)
        {
            var customer = await _customerRepository.GetCustomerWithPurchases(customerId, cancellationToken);
            return customer;
        }
    }

}
