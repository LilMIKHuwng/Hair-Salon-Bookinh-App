using HairSalon.Contract.Services.Interface;
using HairSalon.Core;
using HairSalon.ModelViews.ApplicationUserModelViews;
using HairSalon.ModelViews.AppointmentModelViews;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace HairSalonBE.API.Controllers
{
	[Authorize(Roles = "User")]
	[Route("api/[controller]")]
	[ApiController]
	public class ApplicationUserController : ControllerBase
	{
		private readonly IAppUserService _appUserService;

		public ApplicationUserController(IAppUserService appUserService)
		{
			_appUserService = appUserService;
		}

		[HttpPost("register")]
		public async Task<ActionResult<AppUserModelView>> CreateAppUser([FromQuery] CreateAppUserModelView model)
		{
			try
			{
				AppUserModelView result = await _appUserService.AddAppUserAsync(model);
				return Ok(result);
			}
			catch (Exception ex)
			{
				return BadRequest(new { Message = ex.Message });
			}
		}

		[HttpGet("all")]
		public async Task<ActionResult<BasePaginatedList<AppUserModelView>>> GetAllApplicationUsers(int pageNumber = 1, int pageSize = 5)
		{
			try
			{
				var result = await _appUserService.GetAllAppUserAsync(pageNumber, pageSize);
				return Ok(result);
			}
			catch (Exception ex)
			{
				return BadRequest(new { Message = ex.Message });
			}
		}

		[HttpGet("{id}")]
		public async Task<ActionResult<AppUserModelView>> GetApplicationUserById(string id)
		{
			try
			{
				var result = await _appUserService.GetAppUserAsync(id);
				return Ok(result);
			}
			catch (Exception ex)
			{
				return NotFound(new { Message = ex.Message });
			}
		}

		[HttpPut("{id}")]
		public async Task<IActionResult> UpdateApplicationUser(string id, [FromQuery] UpdateAppUserModelView model)
		{
			try
			{
				AppUserModelView result = await _appUserService.UpdateAppUserAsync(id, model);
				return Ok(result);
			}
			catch (Exception ex)
			{
				return BadRequest(new { Message = ex.Message });
			}
		}

		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteApplicationUser(string id)
		{
			try
			{
				string result = await _appUserService.DeleteAppUserAsync(id);
				return Ok(result);
			}
			catch (Exception ex)
			{
				return BadRequest(new { Message = ex.Message });
			}
		}
    }
}
