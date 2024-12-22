using BookMe.Domain.Interfaces;
using BookMe.Infrastructure.Persistence;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace BookMe.Application.Service.Commands.CreateService
{
    public class CreateServiceCommandValidator : AbstractValidator<CreateServiceCommand>
    {
       

        public CreateServiceCommandValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Nazwa jest wymagana.")
                .MaximumLength(100).WithMessage("Nazwa nie może przekraczać 100 znaków.");

            RuleFor(x => x.Description)
                .MaximumLength(1000).WithMessage("Opis nie może przekraczać 1000 znaków.");

            RuleFor(x => x.ServiceCategoryId)
                .GreaterThan(0).WithMessage("Wybierz kategorię usługi.");

            RuleFor(x => x.ImageUrl)
            .NotEmpty().When(x => !string.IsNullOrEmpty(x.ImageUrl))
            .Matches(@"^(https?:\/\/)?(www\.)?([a-zA-Z0-9-]+\.)+[a-zA-Z]{2,}(:\d+)?(\/\S*)?$")
            .WithMessage("Podaj prawidłowy URL do pliku graficznego.");

            // Walidacja danych kontaktowych
            RuleFor(x => x.PhoneNumber)
        .NotEmpty().WithMessage("Numer telefonu jest wymagany.")
        .Matches(@"^\+?\d+(-\d+)*$").WithMessage("Numer telefonu jest nieprawidłowy.");

            RuleFor(x => x.City)
                .NotEmpty().WithMessage("Miasto jest wymagane.")
                .MaximumLength(100).WithMessage("Nazwa miasta nie może przekraczać 100 znaków.");

            RuleFor(x => x.Street)
                .NotEmpty().WithMessage("Ulica jest wymagana.")
                .MaximumLength(100).WithMessage("Nazwa ulicy nie może przekraczać 100 znaków.");

            RuleFor(x => x.PostalCode)
                .NotEmpty().WithMessage("Kod pocztowy jest wymagany.")
                .Matches(@"^\d{2}-\d{3}$").WithMessage("Kod pocztowy powinien mieć format XX-XXX.");

     


        }

     
    }
}
