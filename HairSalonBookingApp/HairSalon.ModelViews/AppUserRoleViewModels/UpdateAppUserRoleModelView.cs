﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HairSalon.ModelViews.AppUserRoleViewModels
{
	public class UpdateAppUserRoleModelView
	{
		[Required(ErrorMessage = "User ID is required.")]
		public string UserId { get; set; }

		[Required(ErrorMessage = "Role ID is required.")]
		public string RoleId { get; set; }
	}
}
