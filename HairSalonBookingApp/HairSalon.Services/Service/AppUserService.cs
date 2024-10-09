using AutoMapper;
using Castle.Core.Smtp;
using HairSalon.Contract.Repositories.Entity;
using HairSalon.Contract.Repositories.Interface;
using HairSalon.Contract.Services.Interface;
using HairSalon.Core;
using HairSalon.Core.Utils;
using HairSalon.ModelViews.ApplicationUserModelViews;
using HairSalon.ModelViews.AuthModelViews;
using HairSalon.ModelViews.RoleModelViews;
using HairSalon.Repositories.Entity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using IEmailSender = HairSalon.Contract.Services.Interface.IEmailService;

namespace HairSalon.Services.Service
{
	public class AppUserService : IAppUserService
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUsers> _userManager;
        private static readonly Dictionary<string, (string Otp, DateTime Expiration)> OtpStore = new Dictionary<string, (string, DateTime)>();

        public AppUserService(IUnitOfWork unitOfWork, IMapper mapper, UserManager<ApplicationUsers> userManager)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
            _userManager = userManager;
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

            /*// Generate verification code
            var verificationCode = GenerateVerificationCode();
            await _emailService.SendVerificationEmail(model.Email, verificationCode);

            // Store the verification code
            StoreVerificationCode(model.Email, verificationCode);*/

            return _mapper.Map<AppUserModelView>(newAccount);
		}

       /* private string GenerateVerificationCode(int length = 6)
        {
            var random = new Random();
            return random.Next(0, (int)Math.Pow(10, length)).ToString("D" + length);
        }

        public void StoreVerificationCode(string email, string code)
        {
            _verificationCodes[email] = code;
        }

        public bool VerifyCode(string email, string code)
        {
            return _verificationCodes.TryGetValue(email, out var storedCode) && storedCode == code;
        }*/

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
        public async Task<bool> ForgotPasswordAsync(string email)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(email))
                {

                    return false; // Trả về false nếu email không hợp lệ
                }

                // Tìm kiếm người dùng theo email (không phân biệt chữ hoa chữ thường)
                var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Email.ToLower() == email.ToLower());
                if (user == null)
                {

                    return false; // Trả về false nếu không tìm thấy người dùng
                }

                // Tạo OTP
                var random = new Random();
                string otp = random.Next(100000, 999999).ToString();

                // Thiết lập thời gian hết hạn OTP
                var expirationTime = DateTime.UtcNow.AddMinutes(10);

                // Lưu OTP
                if (OtpStore.ContainsKey(email))
                {
                    OtpStore[email] = (otp, expirationTime);
                }
                else
                {
                    OtpStore.Add(email, (otp, expirationTime));
                }

                // Gửi email OTP
               ;/* await _emailSender.SendOtpEmailAsync(email, otp)*/


                return true; // Trả về true nếu mọi thứ thành công
            }
            catch (Exception ex)
            {

                return false; // Trả về false nếu có ngoại lệ
            }
        }

        // Function to reset the password using the OTP
        public async Task<bool> ResetPasswordAsync(string email, string otp, string newPassword)
        {
            try
            {

                if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(otp) || string.IsNullOrWhiteSpace(newPassword))
                {

                    return false; // Trả về false nếu dữ liệu không hợp lệ
                }

                // Tìm người dùng theo email
                var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Email.ToLower() == email.ToLower());
                if (user == null)
                {

                    return false; // Trả về false nếu không tìm thấy người dùng
                }

                // Kiểm tra tính hợp lệ của OTP
                if (!OtpStore.ContainsKey(email) || OtpStore[email].Otp != otp || OtpStore[email].Expiration < DateTime.UtcNow)
                {

                    return false; // Trả về false nếu OTP không hợp lệ
                }

                // Sử dụng UserManager để đặt lại mật khẩu
                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                var result = await _userManager.ResetPasswordAsync(user, token, newPassword);

                if (!result.Succeeded)
                {

                    return false; // Trả về false nếu việc đặt lại mật khẩu không thành công
                }

                // Xóa OTP khỏi hệ thống sau khi sử dụng
                OtpStore.Remove(email);


                return true;
            }
            catch (Exception ex)
            {

                return false; // Trả về false khi có lỗi không mong đợi
            }

        }

        public async Task<bool> VerifyOtpAsync(string email, string otp)
        {
            try
            {
                // Kiểm tra email và OTP có được cung cấp hay không
                if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(otp))
                {

                    return false;
                }

                // Kiểm tra xem OTP có tồn tại trong hệ thống không
                if (!OtpStore.ContainsKey(email))
                {
                    return false;
                }

                var storedOtp = OtpStore[email];


                // Kiểm tra tính hợp lệ của OTP
                if (storedOtp.Otp != otp || storedOtp.Expiration < DateTime.UtcNow)
                {

                    return false;
                }

                // Nếu OTP hợp lệ, trả về true
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

    }
}
