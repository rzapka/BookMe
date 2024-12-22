using BookMe.Application.Employee.Commands.CreateEmployee;
using FluentValidation.TestHelper;
using Xunit;

namespace BookMe.Application.Employee.Commands.CreateEmployee.Tests
{
    public class CreateEmployeeCommandValidatorTests
    {
        private readonly CreateEmployeeCommandValidator _validator;

        public CreateEmployeeCommandValidatorTests()
        {
            _validator = new CreateEmployeeCommandValidator();
        }

        [Fact]
        public void Validator_ShouldHaveError_WhenFirstNameIsEmpty()
        {
            var command = new CreateEmployeeCommand { FirstName = "" };
            var result = _validator.TestValidate(command);
            result.ShouldHaveValidationErrorFor(x => x.FirstName);
        }

        [Fact]
        public void Validator_ShouldNotHaveError_WhenCommandIsValid()
        {
            var command = new CreateEmployeeCommand
            {
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@example.com",
                Password = "Password123",
                ServiceId = 1
            };

            var result = _validator.TestValidate(command);
            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}
