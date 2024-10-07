using HairSalon.Contract.Services.Interface;
using HairSalon.Core;
using HairSalon.ModelViews.ServiceModelViews;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HairSalonBE.API.Controllers
{
	[Authorize(Roles = "User")]
	[Route("api/[controller]")]
    [ApiController]
    public class ServiceController : ControllerBase
    {
        private readonly IServiceService _serviceService;

        public ServiceController(IServiceService serviceService)
        {
            _serviceService = serviceService;
        }

        [HttpGet("all")]
        public async Task<ActionResult<BasePaginatedList<ServiceModelView>>> GetAllServices(int pageNumber = 1, int pageSize = 5)
        {
            try
            {
                var result = await _serviceService.GetAllServiceAsync(pageNumber, pageSize);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ServiceModelView>> GetServiceById(string id)
        {
            try
            {
                var result = await _serviceService.GetServiceAsync(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return NotFound(new { Message = ex.Message });
            }
        }

        [HttpPost()]
        public async Task<ActionResult<ServiceModelView>> CreateService([FromQuery] CreateServiceModelView model)
        {
            try
            {
                ServiceModelView result = await _serviceService.AddServiceAsync(model);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });



            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateService(string id, [FromQuery] UpdatedServiceModelView model)
        {
            try
            {
                ServiceModelView result = await _serviceService.UpdateServiceAsync(id, model);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteService(string id)
        {
            try
            {
                string result = await _serviceService.DeleteServiceAsync(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }
    }
}
