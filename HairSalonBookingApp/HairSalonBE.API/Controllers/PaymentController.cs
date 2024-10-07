using HairSalon.Contract.Services.Interface;
using HairSalon.Core;
using HairSalon.ModelViews.PaymentModelViews;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HairSalonBE.API.Controllers
{
	[Authorize(Roles = "User")]
	[Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService _paymentService;

        public PaymentController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        [HttpGet("All")]
        public async Task<ActionResult<BasePaginatedList<PaymentModelView>>> GetAllPayments(int pageNumber = 1, int pageSize = 5)
        {
            try
            {
                var result = await _paymentService.GetAllPaymentAsync(pageNumber, pageSize);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PaymentModelView>> GetPaymentById(string id)
        {
            try
            {
                var result = await _paymentService.GetPaymentAsync(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return NotFound(new { Message = ex.Message });
            }
        }

        [HttpPost()]
        public async Task<ActionResult<PaymentModelView>> CreatePayment([FromQuery] CreatePaymentModelView model)
        {
            try
            {
                PaymentModelView result = await _paymentService.AddPaymentAsync(model);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePayment(string id, [FromQuery] UpdatedPaymentModelView model)
        {
            try
            {
                PaymentModelView result = await _paymentService.UpdatePaymentAsync(id, model);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePayment(string id)
        {
            try
            {
                string result = await _paymentService.DeletePaymentpAsync(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }
    }
}
