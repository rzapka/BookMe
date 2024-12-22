using BookMe.Application.ApplicationUser.Commands.RegisterApplicationUser;
using FluentValidation.TestHelper;
using Xunit;

namespace BookMe.Application.ApplicationUser.Commands.RegisterApplicationUser.Tests
{
    public class RegisterApplicationUserCommandValidatorTests
    {
        private readonly RegisterApplicationUserCommandValidator _validator;

        public RegisterApplicationUserCommandValidatorTests()
        {
            _validator = new RegisterApplicationUserCommandValidator();
        }

        [Fact]
        public void Validator_ShouldHaveError_WhenFirstNameIsEmpty()
        {
            var command = new RegisterApplicationUserCommand { FirstName = "" };
            var result = _validator.TestValidate(command);
            result.ShouldHaveValidationErrorFor(x => x.FirstName);
        }

        [Fact]
        public void Validator_ShouldHaveError_WhenPasswordsDoNotMatch()
        {
            var command = new RegisterApplicationUserCommand
            {
                Password = "Password123",
                ConfirmPassword = "DifferentPassword"
            };
            var result = _validator.TestValidate(command);
            result.ShouldHaveValidationErrorFor(x => x.ConfirmPassword);
        }

        [Fact]
        public void Validator_ShouldHaveError_WhenEmailIsInvalid()
        {
            var command = new RegisterApplicationUserCommand { Email = "invalid-email" };
            var result = _validator.TestValidate(command);
            result.ShouldHaveValidationErrorFor(x => x.Email);
        }

        [Fact]
        public void Validator_ShouldNotHaveError_WhenCommandIsValid()
        {
            var command = new RegisterApplicationUserCommand
            {
                FirstName = "John",
                LastName = "Doe",
                Email = "user@example.com",
                Password = "ValidPassword123",
                ConfirmPassword = "ValidPassword123"
            };

            var result = _validator.TestValidate(command);
            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}
