using HairSalon.Contract.Repositories.Entity;
using HairSalon.ModelViews.ServiceAppointmentModelViews;

namespace HairSalon.Contract.Services.Interface;

public interface IServiceAppointment
{
    Task<IList<ServiceAppointmentModelView>> GetAllServiceAppointment();
    Task<IList<ServiceAppointmentModelView>> GetAllServiceAppointmentByServiceId(string serviceId);
    Task<IList<ServiceAppointmentModelView>> GetAllServiceAppointmentByAppointmentID(string appointmentId);
    Task<string> DeleteServiceAppointment(DeleteServiceAppointmentModelView deleteServiceAppointmentModelView);
    Task<List<ServiceAppointmentModelView>> GetAllServiceAppointmentByUserId(string userId);
    Task<string> EditServiceAppointment(EditServiceAppointmentModelView editServiceAppointmentModelView);
    Task<string> CreateServiceAppointment(CreatServiceAppointmentModelView creatServiceAppointmentModelView);
    Task<ServiceAppointmentModelView> GetServiceAppointmentById(String id);
}