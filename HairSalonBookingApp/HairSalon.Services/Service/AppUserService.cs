using AutoMapper;
using HairSalon.Contract.Repositories.Entity;
using HairSalon.Contract.Repositories.Interface;
using HairSalon.Contract.Services.Interface;
using HairSalon.Core;
using HairSalon.Core.Utils;
using HairSalon.ModelViews.ApplicationUserModelViews;
using HairSalon.ModelViews.AuthModelViews;
using HairSalon.ModelViews.RoleModelViews;
using HairSalon.Repositories.Entity;
using Microsoft.EntityFrameworkCore;

namespace HairSalon.Services.Service
{
	public class AppUserService : IAppUserService
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;

		public AppUserService(IUnitOfWork unitOfWork, IMapper mapper)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
		}

		public async Task<AppUserModelView> AddAppUserAsync(CreateAppUserModelView model)
		{
			var userInfo = new UserInfo
            {
                Firstname = model.FirstName,
				Lastname = model.LastName,
            };

            var newAccount = new ApplicationUsers
            {
				Id = Guid.NewGuid(),
                UserName = model.UserName,
                Email = model.Email,
				PhoneNumber = model.PhoneNumber,
                PasswordHash = model.Password,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserInfo = userInfo
            };

            var accountRepositoryCheck = _unitOfWork.GetRepository<ApplicationUsers>();

            var user = await accountRepositoryCheck.Entities.FirstOrDefaultAsync(x => x.UserName == model.UserName);
            if (user != null)
            {
                throw new Exception("Duplicate");
            }

            var accountRepository = _unitOfWork.GetRepository<ApplicationUsers>();
            await accountRepository.InsertAsync(newAccount);
            await _unitOfWork.SaveAsync();

            var roleRepository = _unitOfWork.GetRepository<ApplicationRoles>();
            var userRole = await roleRepository.Entities.FirstOrDefaultAsync(r => r.Name == model.RoleName);
            if (userRole == null)
            {
                throw new Exception("The 'User' role does not exist. Please make sure to create it first.");
            }

            var userRoleRepository = _unitOfWork.GetRepository<ApplicationUserRoles>();
            var applicationUserRole = new ApplicationUserRoles
            {
                UserId = newAccount.Id,    
                RoleId = userRole.Id,      
                CreatedBy = model.UserName,
                CreatedTime = DateTime.UtcNow,
                LastUpdatedBy = model.UserName,
                LastUpdatedTime = DateTime.UtcNow
            };

            await userRoleRepository.InsertAsync(applicationUserRole);
            await _unitOfWork.SaveAsync();

            return _mapper.Map<AppUserModelView>(newAccount);
		}

		public async Task<string> DeleteAppUserAsync(string id)
		{
			if (string.IsNullOrWhiteSpace(id))
			{
				throw new Exception("Please provide a valid Application User ID.");
			}

			ApplicationUsers existingUser = await _unitOfWork.GetRepository<ApplicationUsers>().Entities
				.FirstOrDefaultAsync(s => s.Id == Guid.Parse(id) && !s.DeletedTime.HasValue)
				?? throw new Exception("The Application User cannot be found or has been deleted!");

			existingUser.DeletedTime = DateTimeOffset.UtcNow;
			existingUser.DeletedBy = "claim account";

			_unitOfWork.GetRepository<ApplicationUsers>().Update(existingUser);
			await _unitOfWork.SaveAsync();
			return "Deleted";
		}

		public async Task<BasePaginatedList<AppUserModelView>> GetAllAppUserAsync(int pageNumber, int pageSize)
		{
			IQueryable<ApplicationUsers> roleQuery = _unitOfWork.GetRepository<ApplicationUsers>().Entities
				.Where(p => !p.DeletedTime.HasValue)
				.OrderByDescending(s => s.CreatedTime);

			int totalCount = await roleQuery.CountAsync();

			List<ApplicationUsers> paginatedShops = await roleQuery
				.Skip((pageNumber - 1) * pageSize)
				.Take(pageSize)
				.ToListAsync();

			List<AppUserModelView> appUserModelViews = _mapper.Map<List<AppUserModelView>>(paginatedShops);

			return new BasePaginatedList<AppUserModelView>(appUserModelViews, totalCount, pageNumber, pageSize);
		}

		public async Task<AppUserModelView> GetAppUserAsync(string id)
		{
			if (string.IsNullOrWhiteSpace(id))
			{
				throw new Exception("Please provide a valid Application User ID.");
			}

			ApplicationUsers existingUser = await _unitOfWork.GetRepository<ApplicationUsers>().Entities
				.FirstOrDefaultAsync(s => s.Id == Guid.Parse(id) && !s.DeletedTime.HasValue)
				?? throw new Exception("The Application User cannot be found or has been deleted!");

			return _mapper.Map<AppUserModelView>(existingUser);
		}

		public async Task<AppUserModelView> UpdateAppUserAsync(string id, UpdateAppUserModelView model)
		{
			throw new NotImplementedException();
			// if (string.IsNullOrWhiteSpace(id))
			// {
			// 	throw new Exception("Please provide a valid Application User ID.");
			// }

			// ApplicationUser existingUser = await _unitOfWork.GetRepository<ApplicationUser>().Entities
			// 	.FirstOrDefaultAsync(s => s.Id == Guid.Parse(id) && !s.DeletedTime.HasValue)
			// 	?? throw new Exception("The Application User cannot be found or has been deleted!");

			// UserInfo existingUserInfo = await _unitOfWork.GetRepository<UserInfo>().Entities
			// 	.FirstOrDefaultAsync(s => s.Id == model.UserInfoId && !s.DeletedTime.HasValue)
			// 	?? throw new Exception("The User cannot be found or has been deleted!");

			// _mapper.Map(model, existingUser);

			// // Set additional properties
			// existingUser.UserName = existingUserInfo.;
			// existingUser.LastUpdatedBy = "claim account";
			// existingUser.LastUpdatedTime = DateTimeOffset.UtcNow;

			// _unitOfWork.GetRepository<ApplicationUser>().Update(existingUser);
			// await _unitOfWork.SaveAsync();

			// return _mapper.Map<AppUserModelView>(existingUser);
		}

		public async Task<ApplicationUsers> AuthenticateAsync(LoginModelView model)
        {
            var accountRepository = _unitOfWork.GetRepository<ApplicationUsers>();

            // Tìm người dùng theo Username
            var user = await accountRepository.Entities
                .FirstOrDefaultAsync(x => x.UserName == model.Username);

            if (user == null)
            {
                return null; // Người dùng không tồn tại
            }

            // So sánh mật khẩu (bạn có thể sử dụng cơ chế mã hóa mật khẩu)
            if (model.Password != user.PasswordHash)
            {
                return null; // Mật khẩu không khớp
            }
            // Kiểm tra xem đã tồn tại bản ghi đăng nhập chưa
            var loginRepository = _unitOfWork.GetRepository<ApplicationUserLogins>();
            var existingLogin = await loginRepository.Entities
                .FirstOrDefaultAsync(x => x.UserId == user.Id && x.LoginProvider == "CustomLoginProvider");

            if (existingLogin == null)
            {
                // Nếu chưa có bản ghi đăng nhập, thêm mới
                var loginInfo = new ApplicationUserLogins
                {
                    UserId = user.Id, // UserId từ người dùng đã đăng nhập
                    ProviderKey = user.Id.ToString(),
                    LoginProvider = "CustomLoginProvider", // Hoặc có thể là tên provider khác
                    ProviderDisplayName = "Standard Login",
                    CreatedBy = user.UserName, // Ghi lại ai đã thực hiện đăng nhập
                    CreatedTime = CoreHelper.SystemTimeNow,
                    LastUpdatedBy = user.UserName,
                    LastUpdatedTime = CoreHelper.SystemTimeNow
                };

                await loginRepository.InsertAsync(loginInfo);
                await _unitOfWork.SaveAsync(); // Lưu thay đổi vào cơ sở dữ liệu
            }
            else
            {
                // Nếu bản ghi đăng nhập đã tồn tại, có thể cập nhật thông tin nếu cần
                existingLogin.LastUpdatedBy = user.UserName;
                existingLogin.LastUpdatedTime = CoreHelper.SystemTimeNow;

                await loginRepository.UpdateAsync(existingLogin);
                await _unitOfWork.SaveAsync(); // Lưu thay đổi vào cơ sở dữ liệu
            }

            return user; // Trả về người dùng đã xác thực
        }

    }
}
