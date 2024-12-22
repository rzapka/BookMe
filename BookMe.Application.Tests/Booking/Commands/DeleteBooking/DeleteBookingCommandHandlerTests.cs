using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using BookMe.Application.Booking.Commands.DeleteBooking;
using BookMe.Domain.Entities;
using BookMe.Domain.Interfaces;
using MediatR;
using Moq;
using Xunit;

namespace BookMe.Application.Booking.Commands.DeleteBooking.Tests
{
    public class DeleteBookingCommandHandlerTests
    {
        private readonly Mock<IBookingRepository> _bookingRepositoryMock;
        private readonly DeleteBookingCommandHandler _handler;

        public DeleteBookingCommandHandlerTests()
        {
            _bookingRepositoryMock = new Mock<IBookingRepository>();
            _handler = new DeleteBookingCommandHandler(_bookingRepositoryMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldDeleteBooking_WhenBookingExists()
        {
            // Arrange
            var command = new DeleteBookingCommand { Id = 1 };

            var booking = new Domain.Entities.Booking { Id = 1 };

            _bookingRepositoryMock.Setup(x => x.GetBookingByIdAsync(command.Id))
                .ReturnsAsync(booking);

            _bookingRepositoryMock.Setup(x => x.DeleteBookingAsync(command.Id))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.Equal(Unit.Value, result);
            _bookingRepositoryMock.Verify(x => x.DeleteBookingAsync(command.Id), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldThrowKeyNotFoundException_WhenBookingDoesNotExist()
        {
            // Arrange
            var command = new DeleteBookingCommand { Id = 1 };

            _bookingRepositoryMock.Setup(x => x.GetBookingByIdAsync(command.Id))
                .ReturnsAsync((Domain.Entities.Booking)null);

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() => _handler.Handle(command, CancellationToken.None));
        }
    }
}
