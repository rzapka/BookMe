using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using BookMe.Application.Booking.Commands.UpdateBooking;
using BookMe.Domain.Entities;
using BookMe.Domain.Interfaces;
using FluentValidation.TestHelper;
using Moq;
using Xunit;
namespace BookMe.Application.Booking.Commands.UpdateBooking.Tests
{
    public class UpdateBookingCommandValidatorTests
    {
        private readonly Mock<IBookingRepository> _bookingRepositoryMock;
        private readonly Mock<IServiceRepository> _serviceRepositoryMock;
        private readonly UpdateBookingCommandValidator _validator;

        public UpdateBookingCommandValidatorTests()
        {
            _bookingRepositoryMock = new Mock<IBookingRepository>();
            _serviceRepositoryMock = new Mock<IServiceRepository>();
            _validator = new UpdateBookingCommandValidator(_bookingRepositoryMock.Object, _serviceRepositoryMock.Object);
        }

        [Fact]
        public async Task Validator_ShouldHaveError_WhenStartTimeIsEmpty()
        {
            var command = new UpdateBookingCommand { StartTime = default };
            var result = await _validator.TestValidateAsync(command);
            result.ShouldHaveValidationErrorFor(x => x.StartTime).WithErrorMessage("Data rozpoczęcia jest wymagana.");
        }

        [Fact]
        public async Task Validator_ShouldHaveError_WhenUserHasOverlappingBookings()
        {
            var command = new UpdateBookingCommand
            {
                StartTime = DateTime.UtcNow.AddHours(1),
                EndTime = DateTime.UtcNow.AddHours(2),
                UserId = "user123",
                Id = 1
            };

            _bookingRepositoryMock.Setup(x => x.GetBookingsByUserId("user123"))
                .ReturnsAsync(new List<Domain.Entities.Booking>
                {
                new Domain.Entities.Booking { Id = 2, StartTime = DateTime.UtcNow.AddMinutes(30), EndTime = DateTime.UtcNow.AddHours(1) }
                });

            var result = await _validator.TestValidateAsync(command);
            result.ShouldHaveValidationErrorFor(x => x.StartTime).WithErrorMessage("Masz już umówioną wizytę w wybranym przedziale czasowym.");
        }

        [Fact]
        public async Task Validator_ShouldNotHaveError_WhenCommandIsValid()
        {
            // Arrange
            var now = DateTime.UtcNow;
            var startTime = now.AddDays(1).Date.AddHours(10); // Start: następnego dnia o 10:00
            var endTime = startTime.AddMinutes(60);          // Koniec: o 11:00 (1 godzina później)

            var command = new UpdateBookingCommand
            {
                StartTime = startTime,
                EndTime = endTime,
                EmployeeId = 1,
                UserId = "user123",
                Offer = new Domain.Entities.Offer { Id = 1, Duration = 60, ServiceId = 1 }
            };

            _bookingRepositoryMock.Setup(x => x.GetBookingsByUserId(It.IsAny<string>()))
                .ReturnsAsync(new List<Domain.Entities.Booking>());

            _bookingRepositoryMock.Setup(x => x.GetBookingsByEmployeeId(It.IsAny<int>()))
                .ReturnsAsync(new List<Domain.Entities.Booking>());

            _serviceRepositoryMock.Setup(x => x.GetServiceByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(new Domain.Entities.Service
                {
                    OpeningHours = new List<OpeningHour>
                    {
                new OpeningHour
                {
                    DayOfWeek = startTime.ToString("dddd", new CultureInfo("pl-PL")),
                    OpeningTime = TimeSpan.FromHours(8),  // Otwarcie o 8:00
                    ClosingTime = TimeSpan.FromHours(20), // Zamknięcie o 20:00
                    Closed = false
                }
                    }
                });

            // Act
            var result = await _validator.TestValidateAsync(command);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }




    }
}

