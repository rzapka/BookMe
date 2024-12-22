using FluentValidation;

namespace BookMe.Application.ApplicationUser.Commands.LoginApplicationUser
{
    public class LoginApplicationUserCommandValidator : AbstractValidator<LoginApplicationUserCommand>
    {
        public LoginApplicationUserCommandValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email jest wymagany.")
                .EmailAddress().WithMessage("Nieprawidłowy format adresu email.");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Hasło jest wymagane.");
        }
    }
}
