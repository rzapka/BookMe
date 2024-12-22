using System.Threading;
using System.Threading.Tasks;
using BookMe.Application.Notification.Commands.CreateNotification;
using BookMe.Domain.Entities;
using BookMe.Domain.Interfaces;
using MediatR;
using Moq;
using Xunit;

namespace BookMe.Application.Notification.Commands.CreateNotification.Tests
{
    public class CreateNotificationCommandHandlerTests
    {
        private readonly Mock<INotificationRepository> _notificationRepositoryMock;
        private readonly CreateNotificationCommandHandler _handler;

        public CreateNotificationCommandHandlerTests()
        {
            _notificationRepositoryMock = new Mock<INotificationRepository>();
            _handler = new CreateNotificationCommandHandler(_notificationRepositoryMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldCreateNotification_WhenCommandIsValid()
        {
            // Arrange
            var command = new CreateNotificationCommand
            {
                EmployeeId = 1,
                Message = "Test notification message."
            };

            _notificationRepositoryMock.Setup(x => x.CreateAsync(It.IsAny<Domain.Entities.Notification>()))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.Equal(Unit.Value, result);

            _notificationRepositoryMock.Verify(x => x.CreateAsync(It.Is<Domain.Entities.Notification>(n =>
                n.EmployeeId == command.EmployeeId &&
                n.Message == command.Message)), Times.Once);
        }
    }
}
