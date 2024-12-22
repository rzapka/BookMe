using BookMe.Application.Employee.Commands.UpdateEmployee;
using FluentValidation.TestHelper;
using Xunit;

namespace BookMe.Application.Employee.Commands.UpdateEmployee.Tests
{
    public class UpdateEmployeeCommandValidatorTests
    {
        private readonly UpdateEmployeeCommandValidator _validator;

        public UpdateEmployeeCommandValidatorTests()
        {
            _validator = new UpdateEmployeeCommandValidator();
        }

        [Fact]
        public void Validator_ShouldHaveError_WhenServiceIdIsEmpty()
        {
            var command = new UpdateEmployeeCommand { ServiceId = 0 };
            var result = _validator.TestValidate(command);
            result.ShouldHaveValidationErrorFor(x => x.ServiceId);
        }

        [Fact]
        public void Validator_ShouldHaveError_WhenFirstNameIsEmpty()
        {
            var command = new UpdateEmployeeCommand { FirstName = "" };
            var result = _validator.TestValidate(command);
            result.ShouldHaveValidationErrorFor(x => x.FirstName);
        }

        [Fact]
        public void Validator_ShouldHaveError_WhenLastNameIsEmpty()
        {
            var command = new UpdateEmployeeCommand { LastName = "" };
            var result = _validator.TestValidate(command);
            result.ShouldHaveValidationErrorFor(x => x.LastName);
        }

        [Fact]
        public void Validator_ShouldHaveError_WhenEmailIsInvalid()
        {
            var command = new UpdateEmployeeCommand { Email = "invalid-email" };
            var result = _validator.TestValidate(command);
            result.ShouldHaveValidationErrorFor(x => x.Email);
        }

        [Fact]
        public void Validator_ShouldHaveError_WhenPasswordIsTooShort()
        {
            var command = new UpdateEmployeeCommand { Password = "123" };
            var result = _validator.TestValidate(command);
            result.ShouldHaveValidationErrorFor(x => x.Password);
        }

        [Fact]
        public void Validator_ShouldHaveError_WhenNewPasswordIsTooShort()
        {
            var command = new UpdateEmployeeCommand { NewPassword = "123" };
            var result = _validator.TestValidate(command);
            result.ShouldHaveValidationErrorFor(x => x.NewPassword);
        }

        [Fact]
        public void Validator_ShouldHaveError_WhenAvatarUrlIsInvalid()
        {
            var command = new UpdateEmployeeCommand { AvatarUrl = "invalid-url" };
            var result = _validator.TestValidate(command);
            result.ShouldHaveValidationErrorFor(x => x.AvatarUrl);
        }

        [Fact]
        public void Validator_ShouldNotHaveError_WhenCommandIsValid()
        {
            var command = new UpdateEmployeeCommand
            {
                ServiceId = 1,
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@example.com",
                Password = "Password123",
                NewPassword = "NewPassword123",
                AvatarUrl = "https://example.com/avatar.jpg"
            };
            var result = _validator.TestValidate(command);
            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}
