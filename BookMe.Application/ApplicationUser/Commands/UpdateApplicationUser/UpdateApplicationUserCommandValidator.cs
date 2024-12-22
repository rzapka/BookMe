using FluentValidation;
using BookMe.Application.ApplicationUser.Commands.UpdateApplicationUser;
using BookMe.Application.Helpers;

namespace BookMe.Application.ApplicationUser.Commands
{
    public class UpdateApplicationUserCommandValidator : AbstractValidator<UpdateApplicationUserCommand>
    {
        public UpdateApplicationUserCommandValidator()
        {
            // Walidacja dla FirstName
            RuleFor(x => x.FirstName)
                .NotEmpty().WithMessage("Imię jest wymagane.")
                .MaximumLength(50).WithMessage("Imię nie może przekraczać 50 znaków.");

            // Walidacja dla LastName
            RuleFor(x => x.LastName)
                .NotEmpty().WithMessage("Nazwisko jest wymagane.")
                .MaximumLength(50).WithMessage("Nazwisko nie może przekraczać 50 znaków.");

            // Walidacja dla Email
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email jest wymagany.")
                .EmailAddress().WithMessage("Nieprawidłowy format adresu email.");

            RuleFor(x => x.AvatarUrl)
            .NotEmpty().When(x => !string.IsNullOrEmpty(x.AvatarUrl))
            .Matches(@"^(https?:\/\/)?(www\.)?([a-zA-Z0-9-]+\.)+[a-zA-Z]{2,}(:\d+)?(\/\S*)?$")
            .WithMessage("Podaj prawidłowy URL do pliku graficznego.");

            // Walidacja dla NewPassword (jeśli nie jest puste, stosujemy standardowe zasady dla hasła)
            RuleFor(x => x.NewPassword)
                .MinimumLength(6).WithMessage("Hasło musi mieć co najmniej 6 znaków.")
                .When(x => !string.IsNullOrEmpty(x.NewPassword));

            // Walidacja dla PasswordHash (opcjonalnie, ale stosujemy standardowe zasady dla hasła)
            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Hasło jest wymagane.")
                .MinimumLength(6).WithMessage("Hasło musi mieć co najmniej 6 znaków.")
                .When(x => !string.IsNullOrEmpty(x.Password));

        }

    }
}
