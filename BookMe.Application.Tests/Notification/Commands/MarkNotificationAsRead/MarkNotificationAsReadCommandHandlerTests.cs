using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using BookMe.Application.Notification.Commands.MarkNotificationAsRead;
using BookMe.Domain.Entities;
using BookMe.Domain.Interfaces;
using MediatR;
using Moq;
using Xunit;

namespace BookMe.Application.Notification.Commands.MarkNotificationAsRead.Tests
{
    public class MarkNotificationAsReadCommandHandlerTests
    {
        private readonly Mock<INotificationRepository> _notificationRepositoryMock;
        private readonly MarkNotificationAsReadCommandHandler _handler;

        public MarkNotificationAsReadCommandHandlerTests()
        {
            _notificationRepositoryMock = new Mock<INotificationRepository>();
            _handler = new MarkNotificationAsReadCommandHandler(_notificationRepositoryMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldMarkNotificationAsRead_WhenNotificationExists()
        {
            // Arrange
            var command = new MarkNotificationAsReadCommand { NotificationId = 1 };

            var existingNotification = new Domain.Entities.Notification
            {
                Id = 1,
                IsRead = false
            };

            _notificationRepositoryMock.Setup(x => x.GetByIdAsync(command.NotificationId))
                .ReturnsAsync(existingNotification);

            _notificationRepositoryMock.Setup(x => x.UpdateAsync(existingNotification))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.Equal(Unit.Value, result);

            _notificationRepositoryMock.Verify(x => x.UpdateAsync(It.Is<Domain.Entities.Notification>(n =>
                n.Id == command.NotificationId &&
                n.IsRead == true)), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldThrowKeyNotFoundException_WhenNotificationDoesNotExist()
        {
            // Arrange
            var command = new MarkNotificationAsReadCommand { NotificationId = 1 };

            _notificationRepositoryMock.Setup(x => x.GetByIdAsync(command.NotificationId))
                .ReturnsAsync((Domain.Entities.Notification)null);

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() => _handler.Handle(command, CancellationToken.None));
        }
    }
}
