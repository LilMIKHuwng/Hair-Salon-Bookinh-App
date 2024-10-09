using HairSalon.Contract.Services.Interface;
using HairSalon.Core;
using HairSalon.ModelViews.ApplicationUserModelViews;
using HairSalon.ModelViews.AppointmentModelViews;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using HairSalon.Core.Base;
using HairSalon.Services.Service;
using System.Security.Claims;
using HairSalon.Repositories.Entity;
using HairSalon.Core.Constants;
using HairSalon.Core.Utils;

namespace HairSalonBE.API.Controllers
{
	
	[Route("api/[controller]")]
	[ApiController]
	public class ApplicationUserController : ControllerBase
	{
		private readonly IAppUserService _appUserService;
        private readonly ILogger<AppUserService> _logger;

        public ApplicationUserController(IAppUserService appUserService, ILogger<AppUserService> logger)
		{
			_appUserService = appUserService;
            _logger = logger;
            
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
        /* public async Task Login()
         {
             await HttpContext.ChallengeAsync(GoogleDefaults.AuthenticationScheme, new AuthenticationProperties
             {
                 RedirectUri = Url.Action("GoogleResponse")
             });
         }

         public async Task<IActionResult> GoogleResponse()
         {
             var result = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);

             var claims = result.Principal.Identities.FirstOrDefault().Claims.Select(claim => new
             {
                 claim.Issuer,
                 claim.OriginalIssuer,
                 claim.Type,
                 claim.Value
             });
             return Ok(claims);
         }*/

        /*[HttpGet("logout")]
		public async Task<IActionResult> Logout()
		{
			await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
			return Ok("Đã đăng xuất");
		}*/


        /*[HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromQuery] string email)
        {
            try
            {
                // Gọi phương thức ForgotPasswordAsync
                var result = await _appUserService.ForgotPasswordAsync(email);

                if (result)
                {
                    return Ok("Bạn sẽ nhận được một email đặt lại mật khẩu.");
                }
                else
                {
                    // Nếu không thành công, trả về thông báo lỗi
                    return BadRequest("Không tìm thấy người dùng với email này hoặc đã xảy ra lỗi. Vui lòng kiểm tra lại.");
                }
            }
            catch (Exception ex)
            {
                // Ghi log và trả về lỗi 500 nếu có lỗi không mong đợi
                _logger.LogError(ex, "Lỗi xử lý");
                return StatusCode(500, "Đã xảy ra lỗi trong quá trình xử lý yêu cầu của bạn. Vui lòng thử lại sau.");
            }
        }



        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromQuery] string email, [FromQuery] string otp, [FromQuery] string newPassword)
        {
            try
            {
                var result = await _appUserService.ResetPasswordAsync(email, otp, newPassword);

                if (result)
                {
                    return Ok("Mật khẩu đã được đặt lại thành công.");
                }
                else
                {
                    return BadRequest("Email không tồn tại, OTP không hợp lệ hoặc mật khẩu mới không đáp ứng yêu cầu. Vui lòng kiểm tra lại.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi xảy ra khi xử lý yêu cầu đặt lại mật khẩu.");
                return StatusCode(500, "Đã xảy ra lỗi trong quá trình xử lý yêu cầu của bạn. Vui lòng thử lại sau.");
            }
        }
        [HttpPost("verify-otp")]
        public async Task<IActionResult> VerifyOtp([FromQuery] string email, [FromQuery] string otp)
        {
            try
            {
                var result = await _appUserService.VerifyOtpAsync(email, otp);

                if (result)
                {
                    return Ok("OTP hợp lệ.");
                }
                else
                {
                    return BadRequest("OTP không hợp lệ hoặc đã hết hạn.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi xảy ra khi xử lý yêu cầu kiểm tra OTP.");
                return StatusCode(500, "Đã xảy ra lỗi trong quá trình xử lý yêu cầu của bạn. Vui lòng thử lại sau.");
            }
        }*/

    }
}
