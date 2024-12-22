using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using BookMe.Application.Booking.Commands.UpdateBooking;
using BookMe.Domain.Entities;
using BookMe.Domain.Interfaces;
using FluentValidation;
using MediatR;
using Moq;
using Xunit;

namespace BookMe.Application.Booking.Commands.UpdateBooking.Tests
{
    public class UpdateBookingCommandHandlerTests
    {
        private readonly Mock<IBookingRepository> _bookingRepositoryMock;
        private readonly Mock<IValidator<UpdateBookingCommand>> _validatorMock;
        private readonly UpdateBookingCommandHandler _handler;

        public UpdateBookingCommandHandlerTests()
        {
            _bookingRepositoryMock = new Mock<IBookingRepository>();
            _validatorMock = new Mock<IValidator<UpdateBookingCommand>>();
            _handler = new UpdateBookingCommandHandler(_bookingRepositoryMock.Object, _validatorMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldUpdateBooking_WhenCommandIsValid()
        {
            // Arrange
            var command = new UpdateBookingCommand
            {
                Id = 1,
                StartTime = DateTime.Now.AddHours(2),
                EmployeeId = 2,
                Offer = new Domain.Entities.Offer { Duration = 60 }
            };

            var booking = new Domain.Entities.Booking
            {
                Id = 1,
                StartTime = DateTime.Now,
                EmployeeId = 1
            };

            _bookingRepositoryMock.Setup(x => x.GetBookingByIdAsync(command.Id))
                .ReturnsAsync(booking);

            _validatorMock.Setup(x => x.ValidateAsync(command, CancellationToken.None))
                .ReturnsAsync(new FluentValidation.Results.ValidationResult());

            _bookingRepositoryMock.Setup(x => x.UpdateBookingAsync(booking))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.Equal(Unit.Value, result);
            _bookingRepositoryMock.Verify(x => x.UpdateBookingAsync(It.Is<Domain.Entities.Booking>(b =>
                b.StartTime == command.StartTime &&
                b.EmployeeId == command.EmployeeId)), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldThrowValidationException_WhenValidationFails()
        {
            // Arrange
            var command = new UpdateBookingCommand { Id = 1 };

            _validatorMock.Setup(x => x.ValidateAsync(command, CancellationToken.None))
                .ReturnsAsync(new FluentValidation.Results.ValidationResult(new[]
                {
                    new FluentValidation.Results.ValidationFailure("StartTime", "Invalid start time.")
                }));

            // Act & Assert
            await Assert.ThrowsAsync<ValidationException>(() => _handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_ShouldThrowValidationException_WhenBookingDoesNotExist()
        {
            // Arrange
            var command = new UpdateBookingCommand { Id = 1 };

            _bookingRepositoryMock.Setup(x => x.GetBookingByIdAsync(command.Id))
                .ReturnsAsync((Domain.Entities.Booking)null);

            // Act & Assert
            await Assert.ThrowsAsync<ValidationException>(() => _handler.Handle(command, CancellationToken.None));
        }
    }
}
