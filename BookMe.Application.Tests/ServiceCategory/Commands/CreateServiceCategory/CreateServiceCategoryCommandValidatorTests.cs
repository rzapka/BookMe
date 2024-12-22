using BookMe.Application.ServiceCategory.Commands.CreateServiceCategory;
using FluentValidation.TestHelper;
using Xunit;

namespace BookMe.Application.ServiceCategory.Commands.CreateServiceCategory.Tests
{
    public class CreateServiceCategoryCommandValidatorTests
    {
        private readonly CreateServiceCategoryCommandValidator _validator;

        public CreateServiceCategoryCommandValidatorTests()
        {
            _validator = new CreateServiceCategoryCommandValidator();
        }

        [Fact]
        public void Validator_ShouldHaveError_WhenNameIsEmpty()
        {
            var command = new CreateServiceCategoryCommand { Name = "" };
            var result = _validator.TestValidate(command);
            result.ShouldHaveValidationErrorFor(x => x.Name);
        }

        [Fact]
        public void Validator_ShouldNotHaveError_WhenNameAndImageUrlAreValid()
        {
            var command = new CreateServiceCategoryCommand
            {
                Name = "CategoryName",
                ImageUrl = "https://example.com/image.jpg"
            };
            var result = _validator.TestValidate(command);
            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}
