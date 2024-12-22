using BookMe.Application.Opinion.Commands.CreateOpinion;
using FluentValidation.TestHelper;
using Xunit;

namespace BookMe.Application.Opinion.Commands.CreateOpinion.Tests
{
    public class CreateOpinionCommandValidatorTests
    {
        private readonly CreateOpinionCommandValidator _validator;

        public CreateOpinionCommandValidatorTests()
        {
            _validator = new CreateOpinionCommandValidator();
        }

        [Fact]
        public void Validator_ShouldHaveError_WhenRatingIsOutOfRange()
        {
            var command = new CreateOpinionCommand { Rating = 6 };
            var result = _validator.TestValidate(command);
            result.ShouldHaveValidationErrorFor(x => x.Rating);
        }

        [Fact]
        public void Validator_ShouldHaveError_WhenContentIsEmpty()
        {
            var command = new CreateOpinionCommand { Content = "" };
            var result = _validator.TestValidate(command);
            result.ShouldHaveValidationErrorFor(x => x.Content);
        }

        [Fact]
        public void Validator_ShouldNotHaveError_WhenCommandIsValid()
        {
            var command = new CreateOpinionCommand
            {
                Rating = 5,
                Content = "Excellent!"
            };
            var result = _validator.TestValidate(command);
            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}
