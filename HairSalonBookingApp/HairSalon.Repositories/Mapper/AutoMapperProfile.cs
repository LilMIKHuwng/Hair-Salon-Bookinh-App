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
            CreateMap<CreateServiceModelView, ServiceModelView>();
            
			CreateMap<ApplicationRoles, RoleModelView>();
			CreateMap<ApplicationRoles, CreateRoleModelView>();
			CreateMap<ApplicationRoles, UpdatedRoleModelView>();
			CreateMap<CreateRoleModelView, ApplicationRoles>();
			CreateMap<UpdatedRoleModelView, ApplicationRoles>();

            CreateMap<SalaryPayment, SalaryPaymentModelView>();    
            CreateMap<SalaryPayment, CreateSalaryPaymentModelView>();
            CreateMap<SalaryPayment, UpdatedSalaryPaymentModelView>();
			CreateMap<CreateSalaryPaymentModelView, SalaryPayment>();
			CreateMap<UpdatedSalaryPaymentModelView, SalaryPayment>();

			CreateMap<UserInfo, UserModelView>();
            CreateMap<UserInfo, CreateUserModelView>();
            CreateMap<UserInfo, UpdateUserModelView>();
            CreateMap<CreateUserModelView, UserInfo>();
            CreateMap<UpdateUserModelView, UserInfo>();

			CreateMap<ApplicationUsers, AppUserModelView>();
			CreateMap<ApplicationUsers, CreateAppUserModelView>();
			CreateMap<ApplicationUsers, UpdateAppUserModelView>();
			CreateMap<CreateAppUserModelView, ApplicationUsers>();
			CreateMap<UpdateAppUserModelView, ApplicationUsers>();

			CreateMap<ApplicationUserRoles, AppUserRoleModelView>();
			CreateMap<ApplicationUserRoles, CreateAppUserRoleModelView>();
			CreateMap<ApplicationUserRoles, UpdateAppUserRoleModelView>();
			CreateMap<CreateAppUserRoleModelView, ApplicationUserRoles>();
			CreateMap<UpdateAppUserRoleModelView, ApplicationUserRoles>();
		}
    }
}
