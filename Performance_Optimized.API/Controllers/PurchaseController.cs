using Performance_Optimized.Core.Entities.Business;
using Performance_Optimized.Core.Exceptions;
using Performance_Optimized.Core.Interfaces.IServices;
using Microsoft.AspNetCore.Mvc;

namespace Performance_Optimized.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PurchaseController : ControllerBase
    {
        private readonly IPurchaseService _purchaseService;
        private readonly ILogger<PurchaseController> _logger;

        public PurchaseController(IPurchaseService purchaseService, ILogger<PurchaseController> logger)
        {
            _purchaseService = purchaseService;
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
                var purchases = await _purchaseService.Get(pageNumberValue, pageSizeValue, sortBy, sortOrder, cancellationToken);
                return Ok(purchases);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching all purchases.");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("{purchaseId}")]
        public async Task<IActionResult> GetById(int purchaseId, CancellationToken cancellationToken)
        {
            try
            {
                var purchase = await _purchaseService.GetById(purchaseId, cancellationToken);
                if (purchase == null)
                    return NotFound();

                return Ok(purchase);
            }
            catch (NotFoundException ex)
            {
                _logger.LogWarning(ex, $"Purchase with ID {purchaseId} not found.");
                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while fetching purchase with ID {purchaseId}.");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("get-purchases-by-customer/{customerId}")]
        public async Task<IActionResult> GetPurchasesByCustomer(int customerId)
        {
            try
            {
                var purchase = await _purchaseService.GetPurchasesByCustomer(customerId);
                if (purchase == null)
                    return NotFound();

                return Ok(purchase);
            }
            catch (NotFoundException ex)
            {
                _logger.LogWarning(ex, $"Purchase with customer ID {customerId} not found.");
                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while fetching purchase with customer ID {customerId}.");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] PurchaseCreateViewModel model, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var createdPurchase = await _purchaseService.Create(model, cancellationToken);
                return CreatedAtAction(nameof(GetById), new { id = createdPurchase.Id }, createdPurchase);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating a purchase.");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] PurchaseUpdateViewModel model, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var updatedPurchase = await _purchaseService.Update(model, cancellationToken);
                return Ok(updatedPurchase);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating a purchase.");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
        {
            try
            {
                await _purchaseService.Delete(id, cancellationToken);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while deleting purchase with ID {id}.");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
