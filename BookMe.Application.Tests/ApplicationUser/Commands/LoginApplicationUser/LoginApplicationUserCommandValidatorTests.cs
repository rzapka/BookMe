using BookMe.Application.ApplicationUser.Commands.LoginApplicationUser;
using FluentValidation.TestHelper;
using Xunit;

namespace BookMe.Application.ApplicationUser.Commands.LoginApplicationUser.Tests
{
    public class LoginApplicationUserCommandValidatorTests
    {
        private readonly LoginApplicationUserCommandValidator _validator;

        public LoginApplicationUserCommandValidatorTests()
        {
            _validator = new LoginApplicationUserCommandValidator();
        }

        [Fact]
        public void Validator_ShouldHaveError_WhenEmailIsEmpty()
        {
            var command = new LoginApplicationUserCommand { Email = "" };
            var result = _validator.TestValidate(command);
            result.ShouldHaveValidationErrorFor(x => x.Email);
        }

        [Fact]
        public void Validator_ShouldHaveError_WhenEmailIsInvalid()
        {
            var command = new LoginApplicationUserCommand { Email = "invalid-email" };
            var result = _validator.TestValidate(command);
            result.ShouldHaveValidationErrorFor(x => x.Email);
        }

        [Fact]
        public void Validator_ShouldHaveError_WhenPasswordIsEmpty()
        {
            var command = new LoginApplicationUserCommand { Password = "" };
            var result = _validator.TestValidate(command);
            result.ShouldHaveValidationErrorFor(x => x.Password);
        }

        [Fact]
        public void Validator_ShouldNotHaveError_WhenAllFieldsAreValid()
        {
            var command = new LoginApplicationUserCommand
            {
                Email = "user@example.com",
                Password = "ValidPassword123"
            };

            var result = _validator.TestValidate(command);
            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}
