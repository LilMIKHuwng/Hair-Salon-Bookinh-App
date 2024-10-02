using AutoMapper;
using HairSalon.Contract.Repositories.Entity;
using HairSalon.ModelViews.PaymentModelViews;
using HairSalon.ModelViews.AppointmentModelViews;
using HairSalon.ModelViews.ServiceModelViews;
using HairSalon.ModelViews.RoleModelViews;
using HairSalon.ModelViews.SalaryPaymentModelViews;
using HairSalon.ModelViews.ShopModelViews;
using HairSalon.ModelViews.UserModelViews;
using HairSalon.Repositories.Entity;
using HairSalon.ModelViews.ApplicationUserModelViews;
using HairSalon.ModelViews.AppUserRoleViewModels;

namespace HairSalon.Repositories.Mapper
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Shop, ShopModelView>();
            CreateMap<Shop, CreateShopModelView>();
            CreateMap<Shop, UpdatedShopModelView>();
            CreateMap<UpdatedShopModelView, Shop>();
            CreateMap<CreateShopModelView, Shop>();

            CreateMap<Payment, PaymentModelView>();
            CreateMap<Payment, CreatePaymentModelView>();
            CreateMap<Payment, UpdatedPaymentModelView>();
            CreateMap<UpdatedPaymentModelView, Payment>();
            CreateMap<CreatePaymentModelView, Payment>();

            CreateMap<Appointment, AppointmentModelView>();
            CreateMap<Appointment, CreateAppointmentModelView>();
            CreateMap<Appointment, UpdateAppointmentModelView>();
            CreateMap<CreateAppointmentModelView, Appointment>();
            CreateMap<UpdateAppointmentModelView, Appointment>();
            
            CreateMap<Service, ServiceModelView>();
            CreateMap<Service, CreateServiceModelView>();
            CreateMap<Service, UpdatedServiceModelView>();
            CreateMap<UpdatedServiceModelView, Service>();
            CreateMap<CreateServiceModelView, Service>();
            
			CreateMap<ApplicationRole, RoleModelView>();
			CreateMap<ApplicationRole, CreateRoleModelView>();
			CreateMap<ApplicationRole, UpdatedRoleModelView>();
			CreateMap<CreateRoleModelView, ApplicationRole>();
			CreateMap<UpdatedRoleModelView, ApplicationRole>();

            CreateMap<SalaryPayment, SalaryPaymentModelView>();    
            CreateMap<SalaryPayment, CreateSalaryPaymentModelView>();
            CreateMap<SalaryPayment, UpdatedSalaryPaymentModelView>();
			CreateMap<CreateSalaryPaymentModelView, SalaryPayment>();
			CreateMap<UpdatedSalaryPaymentModelView, SalaryPayment>();

			CreateMap<ApplicationUser, UserModelView>();
            CreateMap<ApplicationUser, CreateUserModelView>();
            CreateMap<ApplicationUser, UpdateUserModelView>();
            CreateMap<CreateUserModelView, ApplicationUser>();
            CreateMap<UpdateUserModelView, ApplicationUser>();

			CreateMap<ApplicationUser, AppUserModelView>();
			CreateMap<ApplicationUser, CreateAppUserModelView>();
			CreateMap<ApplicationUser, UpdateAppUserModelView>();
			CreateMap<CreateAppUserModelView, ApplicationUser>();
			CreateMap<UpdateAppUserModelView, ApplicationUser>();

			CreateMap<ApplicationUserRoles, AppUserRoleModelView>();
			CreateMap<ApplicationUserRoles, CreateAppUserRoleModelView>();
			CreateMap<ApplicationUserRoles, UpdateAppUserRoleModelView>();
			CreateMap<CreateAppUserRoleModelView, ApplicationUserRoles>();
			CreateMap<UpdateAppUserRoleModelView, ApplicationUserRoles>();
		}
    }
}
