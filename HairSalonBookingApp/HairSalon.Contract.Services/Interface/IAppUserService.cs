using HairSalon.Core;
using HairSalon.ModelViews.ApplicationUserModelViews;
using HairSalon.ModelViews.AuthModelViews;
using HairSalon.Repositories.Entity;

namespace HairSalon.Contract.Services.Interface
{
	public interface IAppUserService
	{
		Task<BasePaginatedList<AppUserModelView>> GetAllAppUserAsync(string? userId, int pageNumber, int pageSize);
		Task<string> AddAppUserAsync(CreateAppUserModelView model);
		Task<string> UpdateAppUserAsync(string id, UpdateAppUserModelView model);
		Task<string> DeleteAppUserAsync(string id);
		Task<string> ConfirmEmailAsync(string email, string code);
		Task<ApplicationUsers> AuthenticateAsync(LoginModelView model);
	}
}
