using BookMe.Application.ServiceCategory.Commands.UpdateServiceCategory;
using FluentValidation.TestHelper;
using Xunit;

namespace BookMe.Application.ServiceCategory.Commands.UpdateServiceCategory.Tests
{
    public class UpdateServiceCategoryCommandValidatorTests
    {
        private readonly UpdateServiceCategoryCommandValidator _validator;

        public UpdateServiceCategoryCommandValidatorTests()
        {
            _validator = new UpdateServiceCategoryCommandValidator();
        }

        [Fact]
        public void Validator_ShouldHaveError_WhenNameIsEmpty()
        {
            var command = new UpdateServiceCategoryCommand { Name = "" };
            var result = _validator.TestValidate(command);
            result.ShouldHaveValidationErrorFor(x => x.Name);
        }

        [Fact]
        public void Validator_ShouldNotHaveError_WhenNameAndImageUrlAreValid()
        {
            var command = new UpdateServiceCategoryCommand
            {
                Name = "UpdatedCategoryName",
                ImageUrl = "https://example.com/updated-image.jpg"
            };
            var result = _validator.TestValidate(command);
            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}
