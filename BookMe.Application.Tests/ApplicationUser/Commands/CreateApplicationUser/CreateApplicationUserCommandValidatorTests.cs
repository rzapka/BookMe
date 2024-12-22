using Xunit;
using BookMe.Application.ApplicationUser.Commands.CreateApplicationUser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation.TestHelper;

namespace BookMe.Application.ApplicationUser.Commands.CreateApplicationUser.Tests
{
    public class CreateApplicationUserCommandValidatorTests
    {
        private readonly CreateApplicationUserCommandValidator _validator;

        public CreateApplicationUserCommandValidatorTests()
        {
            _validator = new CreateApplicationUserCommandValidator();
        }

        [Fact]
        public void Validator_ShouldHaveError_WhenFirstNameIsEmpty()
        {
            var command = new CreateApplicationUserCommand { FirstName = "" };
            var result = _validator.TestValidate(command);
            result.ShouldHaveValidationErrorFor(x => x.FirstName);
        }

        [Fact]
        public void Validator_ShouldNotHaveError_WhenFirstNameIsValid()
        {
            var command = new CreateApplicationUserCommand { FirstName = "John" };
            var result = _validator.TestValidate(command);
            result.ShouldNotHaveValidationErrorFor(x => x.FirstName);
        }

        [Fact]
        public void Validator_ShouldHaveError_WhenEmailIsInvalid()
        {
            var command = new CreateApplicationUserCommand { Email = "invalid-email" };
            var result = _validator.TestValidate(command);
            result.ShouldHaveValidationErrorFor(x => x.Email);
        }

        [Fact]
        public void Validator_ShouldNotHaveError_WhenEmailIsValid()
        {
            var command = new CreateApplicationUserCommand { Email = "user@example.com" };
            var result = _validator.TestValidate(command);
            result.ShouldNotHaveValidationErrorFor(x => x.Email);
        }

        [Fact]
        public void Validator_ShouldHaveError_WhenPasswordIsTooShort()
        {
            var command = new CreateApplicationUserCommand { Password = "123" };
            var result = _validator.TestValidate(command);
            result.ShouldHaveValidationErrorFor(x => x.Password);
        }
    }
}