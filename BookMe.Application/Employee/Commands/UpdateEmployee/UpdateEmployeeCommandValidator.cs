using FluentValidation;

namespace BookMe.Application.Employee.Commands.UpdateEmployee
{
    public class UpdateEmployeeCommandValidator : AbstractValidator<UpdateEmployeeCommand>
    {
        public UpdateEmployeeCommandValidator()
        {
            RuleFor(x => x.ServiceId).NotEmpty().WithMessage("Serwis jest wymagany.");
            RuleFor(x => x.FirstName).NotEmpty().WithMessage("Imię jest wymagane.");
            RuleFor(x => x.LastName).NotEmpty().WithMessage("Nazwisko jest wymagane.");
            RuleFor(x => x.Email).NotEmpty().EmailAddress().WithMessage("Email jest wymagany i musi być prawidłowy.");
            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Hasło jest wymagane.")
                .MinimumLength(6).WithMessage("Hasło musi mieć co najmniej 6 znaków.");

            RuleFor(x => x.NewPassword)
              .MinimumLength(6).WithMessage("Hasło musi mieć co najmniej 6 znaków.")
              .When(x => !string.IsNullOrEmpty(x.NewPassword));

            RuleFor(x => x.AvatarUrl)
            .NotEmpty().When(x => !string.IsNullOrEmpty(x.AvatarUrl))
            .Matches(@"^(https?:\/\/)?(www\.)?([a-zA-Z0-9-]+\.)+[a-zA-Z]{2,}(:\d+)?(\/\S*)?$")
            .WithMessage("Podaj prawidłowy URL do pliku graficznego.");
        }
    }
}
