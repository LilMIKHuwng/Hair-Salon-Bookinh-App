using System.ComponentModel.DataAnnotations;

namespace HairSalon.ModelViews.ServiceAppointmentModelViews;

public class EditServiceAppointmentModelView
{
    public string Id { get; set; }
    public string ServiceId { get; set; }
    public string AppointmentId { get; set; }
    public string? Description { get; set; }

	[Required]
	[Range(1, 5)]
	public int Rate { get; set; }

	[MaxLength(255)]
	public string? Comment { get; set; }

}