using BookMe.Application.ApplicationUser;
using BookMe.Application.ApplicationUser.Commands.CreateApplicationUser;
using BookMe.Application.ApplicationUser.Queries.GetApplicationUsers;
using BookMe.Application.Booking.Commands.CreateBooking;
using BookMe.Application.Mappings;
using BookMe.Application.Offer.Commands.CreateOffer;
using BookMe.Application.Offer.Commands.UpdateOffer;
using BookMe.Application.OpeningHours.Commands.CreateOpeningHour;
using BookMe.Application.OpeningHours.Commands.UpdateOpeningHour;
using BookMe.Application.Service.Commands.CreateService;
using BookMe.Application.Service.Commands.UpdateService;
using BookMe.Application.ServiceCategory.Queries.GetAllServiceCategories;
using BookMe.Application.Sieve;
using FluentValidation;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Sieve.Services;


namespace BookMe.Application.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddApplication(this IServiceCollection services)
        {
            // Rejestracja MediatR
            services.AddMediatR(typeof(GetAllServiceCategoriesQuery));

            // Rejestracja kontekstu użytkownika
            services.AddScoped<IUserContext, UserContext>();

            // Rejestracja SieveProcessor do filtrowania i paginacji
            services.AddScoped<ISieveProcessor, ApplicationSieveProcessor>();

            // Rejestracja walidatorów FluentValidation
            services.AddValidatorsFromAssemblyContaining<CreateOfferCommandValidator>()
                   .AddFluentValidationAutoValidation()
                   .AddFluentValidationClientsideAdapters();


            services.AddControllersWithViews(options =>
            {
                options.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true;
                options.ModelValidatorProviders.Clear(); 
            });

            services.AddAutoMapper(typeof(ApplicationUserMappingProfile).Assembly);

          
        }
    }
}
