using HairSalon.Core.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HairSalon.Contract.Repositories.Entity
{
	public class ServiceAppointment : BaseEntity
	{
		public string ServiceId { get; set; }

		[ForeignKey("ServiceId")]
		public virtual Service Service { get; set; }

		public string AppointmentId { get; set; }

		[ForeignKey("AppointmentId")]
		public virtual Appointment Appointment { get; set; }

		[MaxLength(255)]
		public string? Description { get; set; }

		[Required]
		[Range(1, 5)]
		public int Rate { get; set; }

		[MaxLength(255)]
		public string? Comment { get; set; }
	}
}
