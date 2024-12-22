using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookMe.Application.Opinion.Commands.UpdateOpinion
{
    public class UpdateOpinionCommandValidator : AbstractValidator<UpdateOpinionCommand>
    {
        public UpdateOpinionCommandValidator()
        {
            RuleFor(x => x.Rating)
               .InclusiveBetween(1, 5).WithMessage("Ocena musi być w przedziale od 1 do 5.");

            RuleFor(x => x.Content)
                .NotEmpty().WithMessage("Komentarz nie może być pusty.");

 
        }
    }
}
