﻿using System.ComponentModel.DataAnnotations;

namespace HairSalon.ModelViews.UserModelViews
{
	public class UpdateUserModelView
	{
		[Required(ErrorMessage = "Fullname is required.")]
		public string FullName { get; set; }

		[Required(ErrorMessage = "Bank Account is required.")]
		public string BankAccount { get; set; }

		[Required(ErrorMessage = "Bank Account Name is required.")]
		public string BankAccountName { get; set; }

		[Required(ErrorMessage = "Bank is required.")]
		public string Bank { get; set; }
		public int Point { get; set; } = 0;
	}
}
