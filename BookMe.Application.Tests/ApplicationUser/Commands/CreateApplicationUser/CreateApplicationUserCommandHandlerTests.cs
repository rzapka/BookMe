using System.Threading;
using System.Threading.Tasks;
using BookMe.Application.ApplicationUser.Commands.CreateApplicationUser;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Moq;
using Xunit;

namespace BookMe.Application.ApplicationUser.Commands.CreateApplicationUser.Tests
{
    public class CreateApplicationUserCommandHandlerTests
    {
        private readonly Mock<UserManager<Domain.Entities.ApplicationUser>> _userManagerMock;
        private readonly CreateApplicationUserCommandHandler _handler;

        public CreateApplicationUserCommandHandlerTests()
        {
            _userManagerMock = MockUserManager<Domain.Entities.ApplicationUser>();
            _handler = new CreateApplicationUserCommandHandler(_userManagerMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldCreateUser_WhenCommandIsValid()
        {
            // Arrange
            var command = new CreateApplicationUserCommand
            {
                FirstName = "John",
                LastName = "Doe",
                Email = "user@example.com",
                Password = "Password123",
                IsAdmin = true
            };

            _userManagerMock.Setup(x => x.CreateAsync(It.IsAny<Domain.Entities.ApplicationUser>(), command.Password))
                .ReturnsAsync(IdentityResult.Success);

            _userManagerMock.Setup(x => x.AddToRoleAsync(It.IsAny<Domain.Entities.ApplicationUser>(), "Admin"))
                .ReturnsAsync(IdentityResult.Success);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.Equal(Unit.Value, result);
            _userManagerMock.Verify(x => x.CreateAsync(It.Is<Domain.Entities.ApplicationUser>(u =>
                u.FirstName == command.FirstName &&
                u.LastName == command.LastName &&
                u.Email == command.Email), command.Password), Times.Once);

            _userManagerMock.Verify(x => x.AddToRoleAsync(It.IsAny<Domain.Entities.ApplicationUser>(), "Admin"), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldThrowValidationException_WhenUserCreationFails()
        {
            // Arrange
            var command = new CreateApplicationUserCommand
            {
                FirstName = "John",
                LastName = "Doe",
                Email = "user@example.com",
                Password = "Password123",
                IsAdmin = false
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
    }
}
