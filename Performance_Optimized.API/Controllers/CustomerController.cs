using Performance_Optimized.Core.Entities.Business;
using Performance_Optimized.Core.Exceptions;
using Performance_Optimized.Core.Interfaces.IServices;
using Microsoft.AspNetCore.Mvc;

namespace Performance_Optimized.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerService _customerService;
        private readonly ILogger<CustomerController> _logger;

        public CustomerController(ICustomerService customerService, ILogger<CustomerController> logger)
        {
            _customerService = customerService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Get(int? pageNumber, int? pageSize, string? sortBy, string? sortOrder, CancellationToken cancellationToken)
        {
            int pageSizeValue = pageSize ?? 10;
            int pageNumberValue = pageNumber ?? 1;
            sortBy = sortBy ?? "Id";
            sortOrder = sortOrder ?? "desc";

            try
            {
                var customers = await _customerService.Get(pageNumberValue, pageSizeValue, sortBy, sortOrder, cancellationToken);
                return Ok(customers);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching all customers.");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("{customerId}")]
        public async Task<IActionResult> GetById(int customerId, CancellationToken cancellationToken)
        {
            try
            {
                var customer = await _customerService.GetById(customerId, cancellationToken);
                if (customer == null)
                    return NotFound();

                return Ok(customer);
            }
            catch (NotFoundException ex)
            {
                _logger.LogWarning(ex, $"Customer with ID {customerId} not found.");
                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while fetching customer with ID {customerId}.");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("get-customer-with-purchases/{customerId}")]
        public async Task<IActionResult> GetCustomerWithPurchases(int customerId, CancellationToken cancellationToken)
        {
            try
            {
                var customer = await _customerService.GetCustomerWithPurchases(customerId, cancellationToken);
                if (customer == null)
                    return NotFound();

                return Ok(customer);
            }
            catch (NotFoundException ex)
            {
                _logger.LogWarning(ex, $"Customer with ID {customerId} not found.");
                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while fetching customer with ID {customerId}.");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CustomerCreateViewModel model, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var createdCustomer = await _customerService.Create(model, cancellationToken);
                return CreatedAtAction(nameof(GetById), new { id = createdCustomer.Id }, createdCustomer);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating a customer.");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] CustomerUpdateViewModel model, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var updatedCustomer = await _customerService.Update(model, cancellationToken);
                return Ok(updatedCustomer);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating a customer.");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
        {
            try
            {
                await _customerService.Delete(id, cancellationToken);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while deleting customer with ID {id}.");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
