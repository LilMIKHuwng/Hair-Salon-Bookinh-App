using HairSalon.Contract.Repositories.Entity;
using System.ComponentModel.DataAnnotations;

namespace HairSalon.ModelViews.ServiceAppointmentModelViews;

public class ServiceAppointmentModelView
{
    public string? Id { get; set; }
    public string? ServiceId { get; set; }
    public string? AppointmentId { get; set; }
    public string? Description { get; set; }
    public DateTimeOffset CreatedTime { get; set; }
    public string? CreatedBy { get; set; }
	public int Rate { get; set; }
	public string? Comment { get; set; }
	public Service Service { get; set; }
    public Appointment Appointment { get; set; }

    public ServiceAppointmentModelView(ServiceAppointment serviceAppointment)
    {
        Id = serviceAppointment.Id;
        ServiceId = serviceAppointment.ServiceId;
        AppointmentId = serviceAppointment.AppointmentId;
        Description = serviceAppointment.Description;
        Service = serviceAppointment.Service;
        Appointment = serviceAppointment.Appointment;
        CreatedTime = serviceAppointment.CreatedTime;
        CreatedBy = serviceAppointment.CreatedBy;
    }
}