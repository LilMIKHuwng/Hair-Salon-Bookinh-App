using AutoMapper;
using HairSalon.Contract.Repositories.Entity;
using HairSalon.Contract.Repositories.Interface;
using HairSalon.Contract.Services.Interface;
using HairSalon.Core;
using HairSalon.ModelViews.RoleModelViews;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
namespace HairSalon.Services.Service
{
    public class RoleService: IRoleService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
		private readonly IHttpContextAccessor _contextAccessor;

		public RoleService(IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor contextAccessor)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
			_contextAccessor = contextAccessor;
		}

		// Get all roles with optional filters for ID and name, and support pagination
		public async Task<BasePaginatedList<RoleModelView>> GetAllRoleAsync(int pageNumber, int pageSize, string? id, string? name)
		{
			IQueryable<ApplicationRoles> roleQuery = _unitOfWork.GetRepository<ApplicationRoles>().Entities
				.Where(p => !p.DeletedTime.HasValue);

			// Apply filters if provided
			if (!string.IsNullOrWhiteSpace(id))
				roleQuery = roleQuery.Where(p => p.Id == Guid.Parse(id));

			if (!string.IsNullOrWhiteSpace(name))
				roleQuery = roleQuery.Where(p => p.Name.Contains(name));

			// Paginate and return the result
			int totalCount = await roleQuery.CountAsync();
			List<ApplicationRoles> paginatedRoles = await roleQuery
				.OrderByDescending(s => s.CreatedTime)
				.Skip((pageNumber - 1) * pageSize)
				.Take(pageSize)
				.ToListAsync();

			List<RoleModelView> roleModelViews = _mapper.Map<List<RoleModelView>>(paginatedRoles);
			return new BasePaginatedList<RoleModelView>(roleModelViews, totalCount, pageNumber, pageSize);
		}

		// Add a new role
		public async Task<string> AddRoleAsync(CreateRoleModelView model)
		{
			ApplicationRoles newRole = _mapper.Map<ApplicationRoles>(model);
			newRole.CreatedBy = _contextAccessor.HttpContext?.User?.FindFirst("userId")?.Value;
			newRole.CreatedTime = DateTimeOffset.UtcNow;

			await _unitOfWork.GetRepository<ApplicationRoles>().InsertAsync(newRole);
			await _unitOfWork.SaveAsync();

			return "Role successfully added";
		}

		// Update an existing role
		public async Task<string> UpdateRoleAsync(string id, UpdatedRoleModelView model)
		{
			if (string.IsNullOrWhiteSpace(id))
			{
				return "Please provide a valid Role ID.";
			}

			var existingRole = await _unitOfWork.GetRepository<ApplicationRoles>().Entities
				.FirstOrDefaultAsync(s => s.Id == Guid.Parse(id) && !s.DeletedTime.HasValue);

			if (existingRole == null)
			{
				return "The Role cannot be found or has been deleted!";
			}

			// Check if the model is null or if there are no changes to apply
			if (model.Name == null || existingRole.Name == model.Name) 
			{
				return "No changes detected, update skipped.";
			}

			_mapper.Map(model, existingRole);
			existingRole.LastUpdatedBy = _contextAccessor.HttpContext?.User?.FindFirst("userId")?.Value;
			existingRole.LastUpdatedTime = DateTimeOffset.UtcNow;

			_unitOfWork.GetRepository<ApplicationRoles>().Update(existingRole);
			await _unitOfWork.SaveAsync();

			return "Role successfully updated";
		}


		// Soft delete a role
		public async Task<string> DeleteRoleAsync(string id)
		{
			if (string.IsNullOrWhiteSpace(id))
			{
				return "Please provide a valid Role ID.";
			}

			var existingRole = await _unitOfWork.GetRepository<ApplicationRoles>().Entities
				.FirstOrDefaultAsync(s => s.Id == Guid.Parse(id) && !s.DeletedTime.HasValue);

			if (existingRole == null)
			{
				return "The Role cannot be found or has been deleted!";
			}

			existingRole.DeletedTime = DateTimeOffset.UtcNow;
			existingRole.DeletedBy = _contextAccessor.HttpContext?.User?.FindFirst("userId")?.Value;

			_unitOfWork.GetRepository<ApplicationRoles>().Update(existingRole);
			await _unitOfWork.SaveAsync();

			return "Role successfully deleted";
		}
	}
}
