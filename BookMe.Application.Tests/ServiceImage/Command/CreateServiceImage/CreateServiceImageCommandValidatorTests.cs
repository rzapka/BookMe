using BookMe.Application.ServiceImage.Commands.CreateServiceImage;
using FluentValidation.TestHelper;
using Xunit;

namespace BookMe.Application.ServiceImage.Commands.CreateServiceImage.Tests
{
    public class CreateServiceImageCommandValidatorTests
    {
        private readonly CreateServiceImageCommandValidator _validator;

        public CreateServiceImageCommandValidatorTests()
        {
            _validator = new CreateServiceImageCommandValidator();
        }

        [Fact]
        public void Validator_ShouldHaveError_WhenUrlIsEmpty()
        {
            var command = new CreateServiceImageCommand { Url = "" };
            var result = _validator.TestValidate(command);
            result.ShouldHaveValidationErrorFor(x => x.Url);
        }

        [Fact]
        public void Validator_ShouldHaveError_WhenUrlIsInvalid()
        {
            var command = new CreateServiceImageCommand { Url = "invalid-url" };
            var result = _validator.TestValidate(command);
            result.ShouldHaveValidationErrorFor(x => x.Url);
        }

        [Fact]
        public void Validator_ShouldNotHaveError_WhenUrlIsValid()
        {
            var command = new CreateServiceImageCommand { Url = "https://example.com/image.jpg" };
            var result = _validator.TestValidate(command);
            result.ShouldNotHaveValidationErrorFor(x => x.Url);
        }
    }
}
