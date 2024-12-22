using BookMe.Application.Offer.Commands.UpdateOffer;
using FluentValidation.TestHelper;
using Xunit;

namespace BookMe.Application.Offer.Commands.UpdateOffer.Tests
{
    public class UpdateOfferCommandValidatorTests
    {
        private readonly UpdateOfferCommandValidator _validator;

        public UpdateOfferCommandValidatorTests()
        {
            _validator = new UpdateOfferCommandValidator();
        }

        [Fact]
        public void Validator_ShouldHaveError_WhenIdIsZeroOrNegative()
        {
            var command = new UpdateOfferCommand { Id = 0 };
            var result = _validator.TestValidate(command);
            result.ShouldHaveValidationErrorFor(x => x.Id);

            command.Id = -1;
            result = _validator.TestValidate(command);
            result.ShouldHaveValidationErrorFor(x => x.Id);
        }

        [Fact]
        public void Validator_ShouldHaveError_WhenNameIsEmpty()
        {
            var command = new UpdateOfferCommand { Name = "" };
            var result = _validator.TestValidate(command);
            result.ShouldHaveValidationErrorFor(x => x.Name);
        }

        [Fact]
        public void Validator_ShouldHaveError_WhenDurationIsOutOfRange()
        {
            var command = new UpdateOfferCommand { Duration = 0 };
            var result = _validator.TestValidate(command);
            result.ShouldHaveValidationErrorFor(x => x.Duration);

            command.Duration = 500;
            result = _validator.TestValidate(command);
            result.ShouldHaveValidationErrorFor(x => x.Duration);
        }

        [Fact]
        public void Validator_ShouldHaveError_WhenPriceIsInvalid()
        {
            var command = new UpdateOfferCommand { Price = -1 };
            var result = _validator.TestValidate(command);
            result.ShouldHaveValidationErrorFor(x => x.Price);
        }

        [Fact]
        public void Validator_ShouldNotHaveError_WhenCommandIsValid()
        {
            var command = new UpdateOfferCommand
            {
                Id = 1,
                Name = "ValidOffer",
                Duration = 60,
                Price = 49.99m
            };
            var result = _validator.TestValidate(command);
            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}
