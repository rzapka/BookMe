using System.Threading;
using System.Threading.Tasks;
using BookMe.Application.ApplicationUser.Commands.LoginApplicationUser;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Moq;
using Xunit;

namespace BookMe.Application.ApplicationUser.Commands.LoginApplicationUser.Tests
{
    public class LoginApplicationUserCommandHandlerTests
    {
        private readonly Mock<SignInManager<Domain.Entities.ApplicationUser>> _signInManagerMock;
        private readonly LoginApplicationUserCommandHandler _handler;

        public LoginApplicationUserCommandHandlerTests()
        {
            _signInManagerMock = MockSignInManager<Domain.Entities.ApplicationUser>(); // Określenie typu
            _handler = new LoginApplicationUserCommandHandler(_signInManagerMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldThrowValidationException_WhenLoginFails()
        {
            // Arrange
            var command = new LoginApplicationUserCommand
            {
                Email = "user@example.com",
                Password = "InvalidPassword"
            };

            _signInManagerMock.Setup(x => x.PasswordSignInAsync(command.Email, command.Password, false, false))
                .ReturnsAsync(SignInResult.Failed);

            // Act & Assert
            await Assert.ThrowsAsync<ValidationException>(() => _handler.Handle(command, CancellationToken.None));
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
