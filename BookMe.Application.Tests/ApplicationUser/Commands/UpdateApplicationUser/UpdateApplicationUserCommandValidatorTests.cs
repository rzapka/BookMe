using BookMe.Application.ApplicationUser.Commands.UpdateApplicationUser;
using FluentValidation.TestHelper;
using Xunit;

namespace BookMe.Application.ApplicationUser.Commands.UpdateApplicationUser.Tests
{
    public class UpdateApplicationUserCommandValidatorTests
    {
        private readonly UpdateApplicationUserCommandValidator _validator;

        public UpdateApplicationUserCommandValidatorTests()
        {
            _validator = new UpdateApplicationUserCommandValidator();
        }

        [Fact]
        public void Validator_ShouldHaveError_WhenFirstNameIsEmpty()
        {
            var command = new UpdateApplicationUserCommand { FirstName = "" };
            var result = _validator.TestValidate(command);
            result.ShouldHaveValidationErrorFor(x => x.FirstName);
        }

        [Fact]
        public void Validator_ShouldHaveError_WhenEmailIsInvalid()
        {
            var command = new UpdateApplicationUserCommand { Email = "invalid-email" };
            var result = _validator.TestValidate(command);
            result.ShouldHaveValidationErrorFor(x => x.Email);
        }

        [Fact]
        public void Validator_ShouldNotHaveError_WhenEmailIsValid()
        {
            var command = new UpdateApplicationUserCommand { Email = "user@example.com" };
            var result = _validator.TestValidate(command);
            result.ShouldNotHaveValidationErrorFor(x => x.Email);
        }

        [Fact]
        public void Validator_ShouldHaveError_WhenNewPasswordIsTooShort()
        {
            var command = new UpdateApplicationUserCommand { NewPassword = "123" };
            var result = _validator.TestValidate(command);
            result.ShouldHaveValidationErrorFor(x => x.NewPassword);
        }

        [Fact]
        public void Validator_ShouldNotHaveError_WhenAllFieldsAreValid()
        {
            var command = new UpdateApplicationUserCommand
            {
                FirstName = "John",
                LastName = "Doe",
                Email = "user@example.com",
                NewPassword = "StrongPass123"
            };
            var result = _validator.TestValidate(command);
            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}
