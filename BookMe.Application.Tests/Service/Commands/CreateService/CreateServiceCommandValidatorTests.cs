using BookMe.Application.Service.Commands.CreateService;
using FluentValidation.TestHelper;
using Xunit;

namespace BookMe.Application.Service.Commands.CreateService.Tests
{
    public class CreateServiceCommandValidatorTests
    {
        private readonly CreateServiceCommandValidator _validator;

        public CreateServiceCommandValidatorTests()
        {
            _validator = new CreateServiceCommandValidator();
        }

        [Fact]
        public void Validator_ShouldHaveError_WhenNameIsEmpty()
        {
            var command = new CreateServiceCommand { Name = "" };
            var result = _validator.TestValidate(command);
            result.ShouldHaveValidationErrorFor(x => x.Name);
        }

        [Fact]
        public void Validator_ShouldNotHaveError_WhenCommandIsValid()
        {
            var command = new CreateServiceCommand
            {
                Name = "ValidName",
                Description = "ValidDescription",
                City = "City",
                Street = "Street",
                PostalCode = "00-000",
                PhoneNumber = "+123456789"
            };
            var result = _validator.TestValidate(command);
            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}
