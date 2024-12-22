using System.Threading;
using System.Threading.Tasks;
using BookMe.Application.ApplicationUser.Commands.DeleteApplicationUser;
using BookMe.Domain.Interfaces;
using MediatR;
using Moq;
using Xunit;

namespace BookMe.Application.ApplicationUser.Commands.DeleteApplicationUser.Tests
{
    public class DeleteApplicationUserCommandHandlerTests
    {
        private readonly Mock<IApplicationUserRepository> _applicationUserRepositoryMock;
        private readonly DeleteApplicationUserCommandHandler _handler;

        public DeleteApplicationUserCommandHandlerTests()
        {
            _applicationUserRepositoryMock = new Mock<IApplicationUserRepository>();
            _handler = new DeleteApplicationUserCommandHandler(_applicationUserRepositoryMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldCallDeleteAsync_WhenCommandIsValid()
        {
            // Arrange
            var command = new DeleteApplicationUserCommand("123");

            _applicationUserRepositoryMock.Setup(x => x.DeleteAsync(command.Id))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.Equal(Unit.Value, result);
            _applicationUserRepositoryMock.Verify(x => x.DeleteAsync(command.Id), Times.Once);
        }
    }
}
