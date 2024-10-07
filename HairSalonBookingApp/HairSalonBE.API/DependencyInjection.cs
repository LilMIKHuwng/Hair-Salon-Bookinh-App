using HairSalon.Contract.Repositories.Entity;
using HairSalon.Contract.Services.Interface;
using HairSalon.Repositories.Context;
using HairSalon.Repositories.Entity;
using HairSalon.Services;
using HairSalon.Services.Service;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace HairSalonBE.API
{
    public static class DependencyInjection
    {
        public static void AddConfig(this IServiceCollection services, IConfiguration configuration)
        {
            services.ConfigRoute();
            services.AddDatabase(configuration);
            services.AddIdentity();
            services.AddInfrastructure(configuration);
            services.AddServices();
        }

        public static void ConfigRoute(this IServiceCollection services)
        {
            services.Configure<RouteOptions>(options =>
            {
                options.LowercaseUrls = true;
            });
        }
        public static void AddDatabase(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<DatabaseContext>(options =>
            {
                options.UseLazyLoadingProxies().UseSqlServer(configuration.GetConnectionString("HairSalonDb"));
            });
        }

        public static void AddIdentity(this IServiceCollection services)
        {
            services.AddIdentity<ApplicationUsers, ApplicationRoles>(options =>
            {
            })
             .AddEntityFrameworkStores<DatabaseContext>()
             .AddDefaultTokenProviders();
        }

        public static void AddServices(this IServiceCollection services)
        {
            services
                //.AddScoped<IUserService, UserService>()
                .AddScoped<IRoleService, RoleService>()
                .AddScoped<IShopService, ShopService>()
                .AddScoped<ISalaryPaymentService, SalaryPaymentService>()
                .AddScoped<IServiceService, ServiceService>()
                .AddScoped<IAppointmentService, AppointmentService>()
                .AddScoped<IPaymentService, PaymentService>()
                .AddScoped<IAppUserRoleService, AppUserRoleService>()
                .AddScoped<IAppUserService, AppUserService>()
                .AddScoped<IServiceAppointment, ServiceAppointmentService>()
                .AddScoped<TokenService>()
                ;

        }
        
        
    }
}
