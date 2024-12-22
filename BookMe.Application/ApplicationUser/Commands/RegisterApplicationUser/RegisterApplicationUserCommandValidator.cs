using FluentValidation;

namespace BookMe.Application.ApplicationUser.Commands.RegisterApplicationUser
{
    public class RegisterApplicationUserCommandValidator : AbstractValidator<RegisterApplicationUserCommand>
    {
        public RegisterApplicationUserCommandValidator()
        {
            RuleFor(x => x.FirstName)
                .NotEmpty().WithMessage("Imię jest wymagane.")
                .MaximumLength(50).WithMessage("Imię nie może przekraczać 50 znaków.");

            RuleFor(x => x.LastName)
                .NotEmpty().WithMessage("Nazwisko jest wymagane.")
                .MaximumLength(50).WithMessage("Nazwisko nie może przekraczać 50 znaków.");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email jest wymagany.")
                .EmailAddress().WithMessage("Nieprawidłowy format adresu email.");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Hasło jest wymagane.")
                .MinimumLength(6).WithMessage("Hasło musi mieć co najmniej 6 znaków.");

            RuleFor(x => x.ConfirmPassword)
                .NotEmpty().WithMessage("Potwierdzenie hasła jest wymagane.")
                .Equal(x => x.Password).WithMessage("Hasło i potwierdzenie hasła muszą być identyczne.");

            RuleFor(x => x.AvatarUrl)
                .NotEmpty().When(x => !string.IsNullOrEmpty(x.AvatarUrl))
                .Matches(@"^(https?:\/\/)?(www\.)?([a-zA-Z0-9-]+\.)+[a-zA-Z]{2,}(:\d+)?(\/\S*)?$")
                .WithMessage("Podaj prawidłowy URL do pliku graficznego.");
        }
    }
}
