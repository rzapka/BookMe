using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using BookMe.Application.ServiceImage.Commands.UpdateServiceImage;
using BookMe.Domain.Entities;
using BookMe.Domain.Interfaces;
using MediatR;
using Moq;
using Xunit;

namespace BookMe.Application.ServiceImage.Commands.UpdateServiceImage.Tests
{
    public class UpdateServiceImageCommandHandlerTests
    {
        private readonly Mock<IServiceImageRepository> _serviceImageRepositoryMock;
        private readonly UpdateServiceImageCommandHandler _handler;

        public UpdateServiceImageCommandHandlerTests()
        {
            _serviceImageRepositoryMock = new Mock<IServiceImageRepository>();
            _handler = new UpdateServiceImageCommandHandler(_serviceImageRepositoryMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldUpdateServiceImage_WhenCommandIsValid()
        {
            // Arrange
            var command = new UpdateServiceImageCommand
            {
                Id = 1,
                Url = "https://example.com/image.jpg"
            };

            var existingServiceImage = new Domain.Entities.ServiceImage
            {
                Id = 1,
                Url = "https://example.com/old-image.jpg"
            };

            _serviceImageRepositoryMock.Setup(x => x.GetServiceImageByIdAsync(command.Id))
                .ReturnsAsync(existingServiceImage);

            _serviceImageRepositoryMock.Setup(x => x.UpdateServiceImageAsync(existingServiceImage))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.Equal(Unit.Value, result);

            _serviceImageRepositoryMock.Verify(x => x.GetServiceImageByIdAsync(command.Id), Times.Once);
            _serviceImageRepositoryMock.Verify(x => x.UpdateServiceImageAsync(It.Is<Domain.Entities.ServiceImage>(si =>
                si.Id == command.Id && si.Url == command.Url)), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldThrowKeyNotFoundException_WhenServiceImageDoesNotExist()
        {
            // Arrange
            var command = new UpdateServiceImageCommand
            {
                Id = 1,
                Url = "https://example.com/image.jpg"
            };

            _serviceImageRepositoryMock.Setup(x => x.GetServiceImageByIdAsync(command.Id))
                .ReturnsAsync((Domain.Entities.ServiceImage)null);

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() => _handler.Handle(command, CancellationToken.None));

            _serviceImageRepositoryMock.Verify(x => x.GetServiceImageByIdAsync(command.Id), Times.Once);
            _serviceImageRepositoryMock.Verify(x => x.UpdateServiceImageAsync(It.IsAny<Domain.Entities.ServiceImage>()), Times.Never);
        }
    }
}
