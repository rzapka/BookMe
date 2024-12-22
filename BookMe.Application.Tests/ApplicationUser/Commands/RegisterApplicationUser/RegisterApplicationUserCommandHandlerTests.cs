using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BookMe.Application.ApplicationUser.Commands.RegisterApplicationUser;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Moq;
using Xunit;

namespace BookMe.Application.ApplicationUser.Commands.RegisterApplicationUser.Tests
{
    public class RegisterApplicationUserCommandHandlerTests
    {
        private readonly Mock<UserManager<Domain.Entities.ApplicationUser>> _userManagerMock;
        private readonly Mock<SignInManager<Domain.Entities.ApplicationUser>> _signInManagerMock;
        private readonly RegisterApplicationUserCommandHandler _handler;

        public RegisterApplicationUserCommandHandlerTests()
        {
            _userManagerMock = MockUserManager<Domain.Entities.ApplicationUser>();
            _signInManagerMock = MockSignInManager<Domain.Entities.ApplicationUser>();

            _handler = new RegisterApplicationUserCommandHandler(
                _userManagerMock.Object,
                _signInManagerMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldRegisterAndSignInUser_WhenCommandIsValid()
        {
            // Arrange
            var command = new RegisterApplicationUserCommand
            {
                FirstName = "John",
                LastName = "Doe",
                Email = "user@example.com",
                Password = "ValidPassword123",
                ConfirmPassword = "ValidPassword123"
            };

            _userManagerMock.Setup(x => x.CreateAsync(It.IsAny<Domain.Entities.ApplicationUser>(), command.Password))
                .ReturnsAsync(IdentityResult.Success);

            _signInManagerMock.Setup(x => x.SignInAsync(It.IsAny<Domain.Entities.ApplicationUser>(), false, null))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.Equal(Unit.Value, result);

            _userManagerMock.Verify(x => x.CreateAsync(It.Is<Domain.Entities.ApplicationUser>(u =>
                u.FirstName == command.FirstName &&
                u.LastName == command.LastName &&
                u.Email == command.Email), command.Password), Times.Once);

            _signInManagerMock.Verify(x => x.SignInAsync(It.IsAny<Domain.Entities.ApplicationUser>(), false, null), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldThrowValidationException_WhenPasswordsDoNotMatch()
        {
            // Arrange
            var command = new RegisterApplicationUserCommand
            {
                Password = "Password123",
                ConfirmPassword = "DifferentPassword"
            };

            // Act & Assert
            await Assert.ThrowsAsync<ValidationException>(() => _handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_ShouldThrowValidationException_WhenUserCreationFails()
        {
            // Arrange
            var command = new RegisterApplicationUserCommand
            {
                FirstName = "John",
                LastName = "Doe",
                Email = "user@example.com",
                Password = "ValidPassword123",
                ConfirmPassword = "ValidPassword123"
            };

            _userManagerMock.Setup(x => x.CreateAsync(It.IsAny<Domain.Entities.ApplicationUser>(), command.Password))
                .ReturnsAsync(IdentityResult.Failed(new IdentityError { Description = "Error creating user" }));

            // Act & Assert
            await Assert.ThrowsAsync<ValidationException>(() => _handler.Handle(command, CancellationToken.None));

            _userManagerMock.Verify(x => x.CreateAsync(It.IsAny<Domain.Entities.ApplicationUser>(), command.Password), Times.Once);
        }

        private static Mock<UserManager<TUser>> MockUserManager<TUser>() where TUser : class
        {
            var store = new Mock<IUserStore<TUser>>();
            return new Mock<UserManager<TUser>>(store.Object, null, null, null, null, null, null, null, null);
        }

        private static Mock<SignInManager<TUser>> MockSignInManager<TUser>() where TUser : class
        {
            var userStoreMock = new Mock<IUserStore<TUser>>();
            var userManagerMock = new Mock<UserManager<TUser>>(
                userStoreMock.Object, null, null, null, null, null, null, null, null);

            return new Mock<SignInManager<TUser>>(
                userManagerMock.Object,
                new Mock<IHttpContextAccessor>().Object,
                new Mock<IUserClaimsPrincipalFactory<TUser>>().Object,
                null, null, null, null);
        }
    }
}
