﻿using HairSalon.Core.Base;
using HairSalon.Repositories.Entity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HairSalon.Contract.Repositories.Entity
{
	public class Appointment : BaseEntity
	{
		[Required]
		public Guid? UserId { get; set; }

		[ForeignKey("UserId")]

		public virtual ApplicationUser User { get; set; }

		public Guid? StylistId { get; set; }

		public virtual ApplicationUser Stylist { get; set; } 

		[MaxLength(50)]
		public string? StatusForAppointment { get; set; }

		public int PointsEarned { get; set; } = 0;

		[Required]
		public DateTime AppointmentDate { get; set; }

		public virtual ICollection<ServiceAppointment>? ServiceAppointments { get; set; }
	}
}
