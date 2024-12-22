using BookMe.Application.Opinion.Commands.UpdateOpinion;
using FluentValidation.TestHelper;
using Xunit;

namespace BookMe.Application.Opinion.Commands.UpdateOpinion.Tests
{
    public class UpdateOpinionCommandValidatorTests
    {
        private readonly UpdateOpinionCommandValidator _validator;

        public UpdateOpinionCommandValidatorTests()
        {
            _validator = new UpdateOpinionCommandValidator();
        }

        [Fact]
        public void Validator_ShouldHaveError_WhenRatingIsOutOfRange()
        {
            var command = new UpdateOpinionCommand { Rating = 0 };
            var result = _validator.TestValidate(command);
            result.ShouldHaveValidationErrorFor(x => x.Rating);
        }

        [Fact]
        public void Validator_ShouldHaveError_WhenContentIsEmpty()
        {
            var command = new UpdateOpinionCommand { Content = "" };
            var result = _validator.TestValidate(command);
            result.ShouldHaveValidationErrorFor(x => x.Content);
        }

        [Fact]
        public void Validator_ShouldNotHaveError_WhenCommandIsValid()
        {
            var command = new UpdateOpinionCommand
            {
                Rating = 4,
                Content = "Updated content"
            };
            var result = _validator.TestValidate(command);
            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}
