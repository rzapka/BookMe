using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookMe.Application.ServiceCategory.Commands.UpdateServiceCategory
{
    public class UpdateServiceCategoryCommandValidator : AbstractValidator<UpdateServiceCategoryCommand>
    {
        public UpdateServiceCategoryCommandValidator()
        {
            RuleFor(x => x.Name)
              .NotEmpty().WithMessage("Nazwa jest wymagana.")
              .MaximumLength(100).WithMessage("Nazwa nie może przekraczać 100 znaków.");

            RuleFor(x => x.ImageUrl)
              .NotEmpty().When(x => !string.IsNullOrEmpty(x.ImageUrl))
              .Matches(@"^(https?:\/\/)?(www\.)?([a-zA-Z0-9-]+\.)+[a-zA-Z]{2,}(:\d+)?(\/\S*)?$")
              .WithMessage("Podaj prawidłowy URL do pliku graficznego.");
        }
    }
}
