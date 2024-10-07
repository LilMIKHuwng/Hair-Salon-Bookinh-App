using AutoMapper;
using HairSalon.Contract.Repositories.Entity;
using HairSalon.Contract.Repositories.Interface;
using HairSalon.Contract.Services.Interface;
using HairSalon.ModelViews.ServiceAppointmentModelViews;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace HairSalon.Services.Service;

public class ServiceAppointmentService : IServiceAppointment
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;


    public ServiceAppointmentService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IList<ServiceAppointmentModelView>> GetAllServiceAppointment()
    {
        IQueryable<ServiceAppointment> serviceAppointments =
            _unitOfWork.GetRepository<ServiceAppointment>()
                .Entities.Where(entity => !entity.DeletedTime.HasValue)
                .OrderByDescending(o => o.CreatedTime);

        IList<ServiceAppointment> appointments = await serviceAppointments.ToListAsync();

        return _mapper.Map<List<ServiceAppointmentModelView>>(appointments);
    }


    public async Task<IList<ServiceAppointmentModelView>> GetAllServiceAppointmentByServiceId(
        string serviceId)
    {
        if (serviceId.IsNullOrEmpty())
        {
            throw new Exception("service Id null: ");
        }

        IQueryable<ServiceAppointment> serviceAppointments = _unitOfWork.GetRepository<ServiceAppointment>()
            .Entities.Where(entity => entity.Service.Id == serviceId)
            .OrderByDescending(entity => entity.CreatedTime);

        if (serviceAppointments == null)
        {
            throw new Exception("service not found: ");
        }

        IList<ServiceAppointment> appointments = await serviceAppointments.ToListAsync();
        return _mapper.Map<List<ServiceAppointmentModelView>>(appointments);
    }

    public async Task<IList<ServiceAppointmentModelView>> GetAllServiceAppointmentByAppointmentID
        (string appointmentId)
    {
        if (appointmentId.IsNullOrEmpty())
        {
            throw new Exception("appointment Id null:");
        }

        IQueryable<ServiceAppointment> serviceAppointments = _unitOfWork.GetRepository<ServiceAppointment>()
            .Entities.Where(entity => entity.Appointment.Id == appointmentId)
            .OrderByDescending(entity => entity.CreatedTime);

        if (serviceAppointments == null)
        {
            throw new Exception("appointment not found:");
        }

        IList<ServiceAppointment> appointments = await serviceAppointments.ToListAsync();
        return _mapper.Map<List<ServiceAppointmentModelView>>(appointments);
    }


    public async Task<string> DeleteServiceAppointment(
        DeleteServiceAppointmentModelView deleteServiceAppointmentModelView)
    {
        if (deleteServiceAppointmentModelView.Id.IsNullOrEmpty())
        {
            throw new Exception("id null");
        }

        var serviceAppointment = await _unitOfWork.GetRepository<ServiceAppointment>()
            .GetByIdAsync(deleteServiceAppointmentModelView.Id);

        if (serviceAppointment == null)
        {
            throw new Exception("appointment not found:");
        }

        serviceAppointment.DeletedBy = deleteServiceAppointmentModelView.DeletedBy;
        serviceAppointment.DeletedTime = DateTimeOffset.UtcNow;

        await _unitOfWork.GetRepository<ServiceAppointment>().UpdateAsync(serviceAppointment);
        await _unitOfWork.SaveAsync();
        return "Deleted successfully";
    }


    public async Task<List<ServiceAppointmentModelView>> GetAllServiceAppointmentByUserId(string userId)
    {
        if (userId.IsNullOrEmpty())
        {
            throw new Exception("id User null:");
        }

        IQueryable<ServiceAppointment> serviceAppointments = _unitOfWork.GetRepository<ServiceAppointment>()
            .Entities.Where(entity => entity.Appointment.UserId.Equals(userId) )
            .OrderByDescending(entity => !entity.DeletedTime.HasValue)
            .ThenByDescending(entity => entity.CreatedTime);

        IList<ServiceAppointment> appointments = await serviceAppointments.ToListAsync();

        return _mapper.Map<List<ServiceAppointmentModelView>>(appointments);
    }

    public async Task<string> EditServiceAppointment(EditServiceAppointmentModelView editServiceAppointmentModelView)
    {
        if (editServiceAppointmentModelView == null)
        {
            throw new Exception("serviceAppointmentModelView must not be null");
        }

        if (editServiceAppointmentModelView.Id.IsNullOrEmpty() &&
            editServiceAppointmentModelView.AppointmentId.IsNullOrEmpty() &&
            editServiceAppointmentModelView.ServiceId.IsNullOrEmpty())
        {
            throw new Exception("Id,AppointmentId or ServiceId null");
        }

        var service = _unitOfWork
            .GetRepository<Contract.Repositories.Entity.Service>()
            .GetById(editServiceAppointmentModelView.ServiceId);

        var appointment =
            _unitOfWork.GetRepository<Appointment>().GetById(editServiceAppointmentModelView.AppointmentId);


        var serviceAppointment = await _unitOfWork.GetRepository<ServiceAppointment>()
            .GetByIdAsync(editServiceAppointmentModelView.Id);

        if (serviceAppointment == null)
        {
            throw new Exception("service not found");
        }

        if (service == null)
        {
            throw new Exception("service not found");
        }

        if (appointment == null)
        {
            throw new Exception("appointment not found");
        }

        serviceAppointment.AppointmentId = editServiceAppointmentModelView.AppointmentId;
        serviceAppointment.Appointment = appointment;
        serviceAppointment.Service = service;
        serviceAppointment.ServiceId = editServiceAppointmentModelView.ServiceId;
        serviceAppointment.LastUpdatedTime = DateTimeOffset.UtcNow;
        serviceAppointment.LastUpdatedBy = service.ShopId;
        serviceAppointment.Description = editServiceAppointmentModelView.Description;

        await _unitOfWork.GetRepository<ServiceAppointment>().UpdateAsync(serviceAppointment);
        await _unitOfWork.GetRepository<ServiceAppointment>().SaveAsync();
        return "Edit successfully";
    }

    public async Task<string> CreateServiceAppointment(
        CreatServiceAppointmentModelView creatServiceAppointmentModelView)
    {
        if (creatServiceAppointmentModelView == null)
        {
            throw new Exception("Entity is null");
        }

        if (creatServiceAppointmentModelView.AppointmentId.IsNullOrEmpty() &&
            creatServiceAppointmentModelView.ServiceId.IsNullOrEmpty())
        {
            throw new Exception("AppointmentId or ServiceId null");
        }

        var service = _unitOfWork
            .GetRepository<Contract.Repositories.Entity.Service>()
            .GetById(creatServiceAppointmentModelView.ServiceId);

        var appointment =
            _unitOfWork.GetRepository<Appointment>().GetById(creatServiceAppointmentModelView.AppointmentId);

        if (service == null)
        {
            throw new KeyNotFoundException($"Service not found with {creatServiceAppointmentModelView.ServiceId}");
        }

        if (appointment == null)
        {
            throw new KeyNotFoundException($"Service not found with {creatServiceAppointmentModelView.AppointmentId}");
        }

        ServiceAppointment serviceAppointment = new ServiceAppointment()
        {
            ServiceId = creatServiceAppointmentModelView.ServiceId,
            AppointmentId = creatServiceAppointmentModelView.AppointmentId,
            Appointment = appointment,
            Service = service,
            CreatedTime = DateTimeOffset.UtcNow,
            CreatedBy = service.ShopId,
            Description = creatServiceAppointmentModelView.Description
        };
        await _unitOfWork.GetRepository<ServiceAppointment>().InsertAsync(serviceAppointment);
        await _unitOfWork.GetRepository<ServiceAppointment>().SaveAsync();
        //return _mapper.Map<ServiceAppointment>(serviceAppointment);
        return "Created successfully";
    }


    public async Task<ServiceAppointmentModelView> GetServiceAppointmentById(string id)
    {
        if (id.IsNullOrEmpty())
        {
            throw new KeyNotFoundException("id null:");
        }

        var serviceAppointment = _unitOfWork.GetRepository<ServiceAppointment>().GetById(id);
        if (serviceAppointment == null)
        {
            throw new Exception("ServiceAppointment not found: " + id);
        }

        if (serviceAppointment.DeletedTime.HasValue)
        {
            throw new Exception("ServiceAppointment not Exist: " + id);
        }

        return new ServiceAppointmentModelView(serviceAppointment);
    }
}