using System.Threading;
using System.Threading.Tasks;
using BookMe.Application.ServiceImage.Commands.DeleteServiceImage;
using BookMe.Domain.Interfaces;
using MediatR;
using Moq;
using Xunit;

namespace BookMe.Application.ServiceImage.Commands.DeleteServiceImage.Tests
{
    public class DeleteServiceImageCommandHandlerTests
    {
        private readonly Mock<IServiceImageRepository> _serviceImageRepositoryMock;
        private readonly DeleteServiceImageCommandHandler _handler;

        public DeleteServiceImageCommandHandlerTests()
        {
            _serviceImageRepositoryMock = new Mock<IServiceImageRepository>();
            _handler = new DeleteServiceImageCommandHandler(_serviceImageRepositoryMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldCallDeleteServiceImageAsync_WhenCommandIsValid()
        {
            // Arrange
            var command = new DeleteServiceImageCommand
            {
                Id = 1
            };

            _serviceImageRepositoryMock.Setup(x => x.DeleteServiceImageAsync(command.Id))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.Equal(Unit.Value, result);

            _serviceImageRepositoryMock.Verify(x => x.DeleteServiceImageAsync(command.Id), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldNotThrowException_WhenRepositoryMethodSucceeds()
        {
            // Arrange
            var command = new DeleteServiceImageCommand
            {
                Id = 1
            };

            _serviceImageRepositoryMock.Setup(x => x.DeleteServiceImageAsync(command.Id))
                .Returns(Task.CompletedTask);

            // Act & Assert
            var exception = await Record.ExceptionAsync(() => _handler.Handle(command, CancellationToken.None));
            Assert.Null(exception);
        }
    }
}
