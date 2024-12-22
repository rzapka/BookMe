using BookMe.Application.Offer.Commands.CreateOffer;
using FluentValidation.TestHelper;
using Xunit;

namespace BookMe.Application.Offer.Commands.CreateOffer.Tests
{
    public class CreateOfferCommandValidatorTests
    {
        private readonly CreateOfferCommandValidator _validator;

        public CreateOfferCommandValidatorTests()
        {
            _validator = new CreateOfferCommandValidator();
        }

        [Fact]
        public void Validator_ShouldHaveError_WhenNameIsEmpty()
        {
            var command = new CreateOfferCommand { Name = "" };
            var result = _validator.TestValidate(command);
            result.ShouldHaveValidationErrorFor(x => x.Name);
        }

        [Fact]
        public void Validator_ShouldHaveError_WhenDurationIsZero()
        {
            var command = new CreateOfferCommand { Duration = 0 };
            var result = _validator.TestValidate(command);
            result.ShouldHaveValidationErrorFor(x => x.Duration);
        }

        [Fact]
        public void Validator_ShouldHaveError_WhenPriceIsNegative()
        {
            var command = new CreateOfferCommand { Price = -1 };
            var result = _validator.TestValidate(command);
            result.ShouldHaveValidationErrorFor(x => x.Price);
        }

        [Fact]
        public void Validator_ShouldNotHaveError_WhenCommandIsValid()
        {
            var command = new CreateOfferCommand
            {
                Name = "ValidName",
                Duration = 30,
                Price = 19.99m,
                ServiceId = 1
            };
            var result = _validator.TestValidate(command);
            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}
