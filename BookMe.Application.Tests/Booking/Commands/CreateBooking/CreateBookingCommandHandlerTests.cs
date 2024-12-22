using System;
using System.Threading;
using System.Threading.Tasks;
using BookMe.Application.Booking.Commands.CreateBooking;
using BookMe.Domain.Entities;
using BookMe.Domain.Interfaces;
using FluentValidation;
using Moq;
using Xunit;
using FluentValidation.Results;
using MediatR;

namespace BookMe.Application.Booking.Commands.CreateBooking.Tests
{
    public class CreateBookingCommandHandlerTests
    {
        private readonly Mock<IBookingRepository> _bookingRepositoryMock;
        private readonly Mock<IOfferRepository> _offerRepositoryMock;
        private readonly Mock<IValidator<CreateBookingCommand>> _validatorMock;
        private readonly CreateBookingCommandHandler _handler;

        public CreateBookingCommandHandlerTests()
        {
            _bookingRepositoryMock = new Mock<IBookingRepository>();
            _offerRepositoryMock = new Mock<IOfferRepository>();
            _validatorMock = new Mock<IValidator<CreateBookingCommand>>();

            _handler = new CreateBookingCommandHandler(
                _bookingRepositoryMock.Object,
                _offerRepositoryMock.Object,
                _validatorMock.Object
            );
        }

        [Fact]
        public async Task Handle_ShouldCreateBooking_WhenCommandIsValid()
        {
            // Arrange
            var command = new CreateBookingCommand
            {
                EmployeeId = 1,
                OfferId = 2,
                UserId = "user1",
                StartTime = DateTime.Now.AddHours(1)
            };

            var offer = new Domain.Entities.Offer
            {
                Id = 2,
                Duration = 60,
                ServiceId = 70 // Dopasowane ServiceId
            };

            _offerRepositoryMock.Setup(x => x.GetByIdAsync(command.OfferId))
                .ReturnsAsync(offer);

            _validatorMock.Setup(x => x.ValidateAsync(command, CancellationToken.None))
                .ReturnsAsync(new FluentValidation.Results.ValidationResult());

            _bookingRepositoryMock.Setup(x => x.Create(It.IsAny<Domain.Entities.Booking>()))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.Equal(Unit.Value, result);

            _offerRepositoryMock.Verify(x => x.GetByIdAsync(command.OfferId), Times.Once);

            _bookingRepositoryMock.Verify(x => x.Create(It.Is<Domain.Entities.Booking>(b =>
                b.EmployeeId == command.EmployeeId &&
                b.OfferId == command.OfferId &&
                b.UserId == command.UserId &&
                Math.Abs((b.StartTime - command.StartTime).TotalSeconds) < 1 &&
                b.ServiceId == offer.ServiceId
            )), Times.Once);
        }




        [Fact]
        public async Task Handle_ShouldThrowValidationException_WhenOfferDoesNotExist()
        {
            // Arrange
            var command = new CreateBookingCommand { OfferId = 2 };

            _offerRepositoryMock.Setup(x => x.GetByIdAsync(command.OfferId))
                .ReturnsAsync((Domain.Entities.Offer)null);

            // Act & Assert
            await Assert.ThrowsAsync<ValidationException>(() => _handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_ShouldThrowValidationException_WhenValidationFails()
        {
            // Arrange
            var command = new CreateBookingCommand { OfferId = 2 };

            _validatorMock.Setup(x => x.ValidateAsync(command, CancellationToken.None))
                .ReturnsAsync(new ValidationResult(new[]
                {
                    new ValidationFailure("StartTime", "Invalid start time.")
                }));

            // Act & Assert
            await Assert.ThrowsAsync<ValidationException>(() => _handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_ShouldCallSetEndTime()
        {
            // Arrange
            var command = new CreateBookingCommand
            {
                EmployeeId = 1,
                OfferId = 2,
                UserId = "user1",
                StartTime = DateTime.Now.AddHours(1)
            };

            var offer = new Domain.Entities.Offer
            {
                Id = 2,
                Duration = 60,
                ServiceId = 3
            };

            _offerRepositoryMock.Setup(x => x.GetByIdAsync(command.OfferId))
                .ReturnsAsync(offer);

            _validatorMock.Setup(x => x.ValidateAsync(command, CancellationToken.None))
                .ReturnsAsync(new ValidationResult());

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.Equal(command.StartTime.AddMinutes(offer.Duration), command.EndTime);
        }
    }
}
