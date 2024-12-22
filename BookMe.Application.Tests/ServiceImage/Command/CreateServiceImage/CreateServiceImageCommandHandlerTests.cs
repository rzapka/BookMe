using System.Threading;
using System.Threading.Tasks;
using BookMe.Application.ServiceImage.Commands.CreateServiceImage;
using BookMe.Domain.Entities;
using BookMe.Domain.Interfaces;
using MediatR;
using Moq;
using Xunit;

namespace BookMe.Application.ServiceImage.Commands.CreateServiceImage.Tests
{
    public class CreateServiceImageCommandHandlerTests
    {
        private readonly Mock<IServiceImageRepository> _serviceImageRepositoryMock;
        private readonly CreateServiceImageCommandHandler _handler;

        public CreateServiceImageCommandHandlerTests()
        {
            _serviceImageRepositoryMock = new Mock<IServiceImageRepository>();
            _handler = new CreateServiceImageCommandHandler(_serviceImageRepositoryMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldCallAddServiceImageAsync_WhenCommandIsValid()
        {
            // Arrange
            var command = new CreateServiceImageCommand
            {
                Url = "https://example.com/image.jpg",
                ServiceId = 1
            };

            _serviceImageRepositoryMock.Setup(x => x.AddServiceImageAsync(It.IsAny<Domain.Entities.ServiceImage>()))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.Equal(Unit.Value, result);

            _serviceImageRepositoryMock.Verify(x => x.AddServiceImageAsync(It.Is<Domain.Entities.ServiceImage>(si =>
                si.Url == command.Url &&
                si.ServiceId == command.ServiceId)), Times.Once);
        }
    }
}
