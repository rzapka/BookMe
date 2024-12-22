using FluentValidation;

namespace BookMe.Application.Offer.Commands.UpdateOffer
{
    public class UpdateOfferCommandValidator : AbstractValidator<UpdateOfferCommand>
    {
        public UpdateOfferCommandValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("Identyfikator oferty musi być większy od 0.");

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Nazwa oferty jest wymagana.")
                .MaximumLength(100).WithMessage("Nazwa oferty nie może przekraczać 100 znaków.");

            RuleFor(x => x.Duration)
                .GreaterThan(0).WithMessage("Czas trwania musi być większy od 0.")
                .LessThanOrEqualTo(480).WithMessage("Czas trwania nie może przekraczać 480 minut (8 godzin).");

            RuleFor(x => x.Price)
                .GreaterThan(0).WithMessage("Cena musi być większa od 0.")
                .ScalePrecision(2, 18).WithMessage("Cena musi być liczbą dziesiętną z maksymalnie dwoma miejscami po przecinku.");

        }
    }
}
