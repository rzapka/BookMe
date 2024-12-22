using FluentValidation;

namespace BookMe.Application.ServiceImage.Commands.UpdateServiceImage
{
    public class UpdateServiceImageCommandValidator : AbstractValidator<UpdateServiceImageCommand>
    {
        public UpdateServiceImageCommandValidator()
        {
            RuleFor(x => x.Url)
             .NotEmpty().WithMessage("Adres URL obrazka jest wymagany.")
             .Matches(@"^(https?:\/\/)?(www\.)?([a-zA-Z0-9-]+\.)+[a-zA-Z]{2,}(:\d+)?(\/\S*)?$")
             .WithMessage("Podaj prawidłowy URL do pliku graficznego.");

        }
    }
}
