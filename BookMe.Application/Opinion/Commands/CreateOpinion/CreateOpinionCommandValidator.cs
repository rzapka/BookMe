using FluentValidation;

namespace BookMe.Application.Opinion.Commands.CreateOpinion
{
    public class CreateOpinionCommandValidator : AbstractValidator<CreateOpinionCommand>
    {
        public CreateOpinionCommandValidator()
        {
            RuleFor(x => x.Rating)
                .InclusiveBetween(1, 5).WithMessage("Ocena musi być w przedziale od 1 do 5.");

            RuleFor(x => x.Content)
                .NotEmpty().WithMessage("Komentarz nie może być pusty.");

        }
    }
}
