using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using BookMe.Application.Service.Commands.DeleteService;
using BookMe.Domain.Entities;
using BookMe.Domain.Interfaces;
using MediatR;
using Moq;
using Xunit;

namespace BookMe.Application.Service.Commands.DeleteService.Tests
{
    public class DeleteServiceCommandHandlerTests
    {
        private readonly Mock<IServiceRepository> _serviceRepositoryMock;
        private readonly DeleteServiceCommandHandler _handler;

        public DeleteServiceCommandHandlerTests()
        {
            _serviceRepositoryMock = new Mock<IServiceRepository>();
            _handler = new DeleteServiceCommandHandler(_serviceRepositoryMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldDeleteService_WhenCommandIsValid()
        {
            // Arrange
            var command = new DeleteServiceCommand { EncodedName = "service-encoded" };

            var existingService = new Domain.Entities.Service();

            _serviceRepositoryMock.Setup(x => x.GetServiceByEncodedName(command.EncodedName))
                .ReturnsAsync(existingService);

            _serviceRepositoryMock.Setup(x => x.DeleteAsync(existingService))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.Equal(Unit.Value, result);

            _serviceRepositoryMock.Verify(x => x.DeleteAsync(existingService), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldThrowKeyNotFoundException_WhenServiceDoesNotExist()
        {
            // Arrange
            var command = new DeleteServiceCommand { EncodedName = "not-found" };

            _serviceRepositoryMock.Setup(x => x.GetServiceByEncodedName(command.EncodedName))
                .ReturnsAsync((Domain.Entities.Service)null);

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() => _handler.Handle(command, CancellationToken.None));
        }
    }
}
