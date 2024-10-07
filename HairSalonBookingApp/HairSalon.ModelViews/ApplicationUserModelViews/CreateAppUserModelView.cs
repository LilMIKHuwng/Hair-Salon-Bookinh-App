using System.ComponentModel.DataAnnotations;

namespace HairSalon.ModelViews.ApplicationUserModelViews
{
	public class CreateAppUserModelView
	{
		[Required(ErrorMessage = "First name is required.")]
        public string FirstName { get; set; }
		[Required(ErrorMessage = "Last name is required.")]
        public string LastName { get; set; }
		[Required(ErrorMessage = "Username is required.")]
		public string UserName { get; set; }
		[Required(ErrorMessage = "Email is required.")]
		[EmailAddress(ErrorMessage = "Invalid Email Address.")]
		public string Email { get; set; }

		[Required(ErrorMessage = "Password is required.")]
		public string Password { get; set; }

		[Required(ErrorMessage = "Phone number is required.")]
		[Phone(ErrorMessage = "Invalid Phone Number.")]
		[StringLength(20, ErrorMessage = "Phone number cannot be longer than 20 characters.")]
		public string PhoneNumber { get; set; }
		public string RoleName { get; set; }
	}
}
