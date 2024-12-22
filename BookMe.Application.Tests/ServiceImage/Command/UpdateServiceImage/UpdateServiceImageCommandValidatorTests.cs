using BookMe.Application.ServiceImage.Commands.UpdateServiceImage;
using FluentValidation.TestHelper;
using Xunit;

namespace BookMe.Application.ServiceImage.Commands.UpdateServiceImage.Tests
{
    public class UpdateServiceImageCommandValidatorTests
    {
        private readonly UpdateServiceImageCommandValidator _validator;

        public UpdateServiceImageCommandValidatorTests()
        {
            _validator = new UpdateServiceImageCommandValidator(); // Używamy poprawnego walidatora
        }

        [Fact]
        public void Validator_ShouldHaveError_WhenUrlIsEmpty()
        {
            var command = new UpdateServiceImageCommand { Url = "" };
            var result = _validator.TestValidate(command);
            result.ShouldHaveValidationErrorFor(x => x.Url);
        }

        [Fact]
        public void Validator_ShouldHaveError_WhenUrlIsInvalid()
        {
            var command = new UpdateServiceImageCommand { Url = "invalid-url" };
            var result = _validator.TestValidate(command);
            result.ShouldHaveValidationErrorFor(x => x.Url);
        }

        [Fact]
        public void Validator_ShouldNotHaveError_WhenUrlIsValid()
        {
            var command = new UpdateServiceImageCommand { Url = "https://example.com/image.jpg" };
            var result = _validator.TestValidate(command);
            result.ShouldNotHaveValidationErrorFor(x => x.Url);
        }
    }
}
