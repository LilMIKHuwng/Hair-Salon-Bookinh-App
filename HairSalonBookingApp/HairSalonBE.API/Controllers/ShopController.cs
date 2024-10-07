using HairSalon.Contract.Services.Interface;
using HairSalon.Core;
using HairSalon.ModelViews.ShopModelViews;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HairSalonBE.API.Controllers
{
	[Authorize(Roles = "User")]
	[Route("api/[controller]")]
    [ApiController]
    public class ShopController : ControllerBase
    {
        private readonly IShopService _shopService;

        public ShopController(IShopService shopService)
        {
            _shopService = shopService;
        }

        [HttpGet("all")]
        public async Task<ActionResult<BasePaginatedList<ShopModelView>>> GetAllShops(int pageNumber = 1, int pageSize = 5)
        {
            try
            {
                var result = await _shopService.GetAllShopAsync(pageNumber, pageSize);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ShopModelView>> GetShopById(string id)
        {
            try
            {
                var result = await _shopService.GetShopAsync(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return NotFound(new { Message = ex.Message });
            }
        }

        [HttpPost()]
        public async Task<ActionResult<ShopModelView>> CreateShop([FromQuery] CreateShopModelView model)
        {
            try
            {
                ShopModelView result = await _shopService.AddShopAsync(model);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateShop(string id, [FromQuery] UpdatedShopModelView model)
        {
            try
            {
                ShopModelView result = await _shopService.UpdateShopAsync(id, model);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteShop(string id)
        {
            try
            {
                string result = await _shopService.DeleteShopAsync(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }
    }
}
