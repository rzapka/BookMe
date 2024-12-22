using BookMe.Domain.Entities;
using BookMe.Domain.Interfaces;
using BookMe.Infrastructure.Persistence;
using BookMe.Infrastructure.Repositories;
using BookMe.Infrastructure.Seeders;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BookMe.Infrastructure.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddInfrastucture(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<BookMeDbContext>(options => options.UseSqlServer(
                configuration.GetConnectionString("BookMe")));


            services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<BookMeDbContext>();


            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();


            services.AddScoped<UserSeeder>();
            services.AddScoped<EmployeeSeeder>();
            services.AddScoped<ServiceCategorySeeder>();
            services.AddScoped<ServiceSeeder>();
            services.AddScoped<OfferSeeder>();
            services.AddScoped<BookingSeeder>();
            services.AddScoped<OpinionSeeder>();
            services.AddScoped<OpeningHoursSeeder>();
            services.AddScoped<ServiceImageSeeder>();
            

            services.AddScoped<IServiceCategoryRepository, ServiceCategoryRepository>();
            services.AddScoped<IServiceRepository, ServiceRepository>();
            services.AddScoped<IBookingRepository, BookingRepository>();
            services.AddScoped<IOfferRepository, OfferRepository>();
            services.AddScoped<IEmployeeRepository, EmployeeRepository>();
            services.AddScoped<IOpinionRepository, OpinionRepository>();
            services.AddScoped<INotificationRepository, NotificationRepository>();
            services.AddScoped<IApplicationUserRepository, ApplicationUserRepository>();
            services.AddScoped<IOpeningHoursRepository, OpeningHoursRepository>();
            services.AddScoped<IServiceImageRepository, ServiceImageRepository>();
        }
    }
}
