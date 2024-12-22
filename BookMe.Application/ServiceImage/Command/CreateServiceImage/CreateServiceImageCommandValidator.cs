using FluentValidation;

namespace BookMe.Application.ServiceImage.Commands.CreateServiceImage
{
    public class CreateServiceImageCommandValidator : AbstractValidator<CreateServiceImageCommand>
    {
        public CreateServiceImageCommandValidator()
        {
            RuleFor(x => x.Url)
              .NotEmpty().WithMessage("Adres URL obrazka jest wymagany.")
              .Matches(@"^(https?:\/\/)?(www\.)?([a-zA-Z0-9-]+\.)+[a-zA-Z]{2,}(:\d+)?(\/\S*)?$")
              .WithMessage("Podaj prawidłowy URL do pliku graficznego.");

        }
    }
}
