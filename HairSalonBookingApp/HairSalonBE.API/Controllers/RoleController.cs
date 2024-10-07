using HairSalon.Contract.Services.Interface;
using HairSalon.Core;
using HairSalon.ModelViews.RoleModelViews;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HairSalonBE.API.Controllers
{
	[Authorize(Roles = "Admin")]
	[Route("api/[controller]")]
	[ApiController]
	public class RoleController : ControllerBase
	{
		private readonly IRoleService _roleService;

		public RoleController(IRoleService roleService)
		{
			_roleService = roleService;
		}

		[HttpGet("all")]
		public async Task<ActionResult<BasePaginatedList<RoleModelView>>> GetAllRoles(int pageNumber = 1, int pageSize = 5,
															[FromQuery] string? id = null, [FromQuery] string? name = null)
		{
			var result = await _roleService.GetAllRoleAsync(pageNumber, pageSize, id, name);
			return Ok(result);
		}

		[HttpPost()]
		public async Task<ActionResult<string>> CreateRole([FromQuery] CreateRoleModelView model)
		{
			string result = await _roleService.AddRoleAsync(model);
			return Ok(new { Message = result });
		}

		[HttpPut("{id}")]
		public async Task<ActionResult<string>> UpdateRole(string id, [FromQuery] UpdatedRoleModelView model)
		{
			string result = await _roleService.UpdateRoleAsync(id, model);
			return Ok(new { Message = result });
		}

		[HttpDelete("{id}")]
		public async Task<ActionResult<string>> DeleteRole(string id)
		{
			string result = await _roleService.DeleteRoleAsync(id);
			return Ok(new { Message = result });
		}
	}
}
