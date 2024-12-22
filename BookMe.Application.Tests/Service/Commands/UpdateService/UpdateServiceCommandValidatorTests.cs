using BookMe.Application.Service.Commands.UpdateService;
using FluentValidation.TestHelper;
using Xunit;

namespace BookMe.Application.Service.Commands.UpdateService.Tests
{
    public class UpdateServiceCommandValidatorTests
    {
        private readonly UpdateServiceCommandValidator _validator;

        public UpdateServiceCommandValidatorTests()
        {
            _validator = new UpdateServiceCommandValidator();
        }

        [Fact]
        public void Validator_ShouldHaveError_WhenNameIsEmpty()
        {
            var command = new UpdateServiceCommand { Name = "" };
            var result = _validator.TestValidate(command);
            result.ShouldHaveValidationErrorFor(x => x.Name);
        }

        [Fact]
        public void Validator_ShouldNotHaveError_WhenCommandIsValid()
        {
            var command = new UpdateServiceCommand
            {
                Name = "UpdatedService",
                City = "City",
                Street = "Main Street",
                PostalCode = "00-000",
                PhoneNumber = "+123456789"
            };
            var result = _validator.TestValidate(command);
            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}
