using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using BookMe.Application.OpeningHours.Commands.UpdateOpeningHour;
using BookMe.Domain.Entities;
using BookMe.Domain.Interfaces;
using MediatR;
using Moq;
using Xunit;

namespace BookMe.Application.OpeningHours.Commands.UpdateOpeningHour.Tests
{
    public class UpdateOpeningHourCommandHandlerTests
    {
        private readonly Mock<IOpeningHoursRepository> _openingHoursRepositoryMock;
        private readonly UpdateOpeningHourCommandHandler _handler;

        public UpdateOpeningHourCommandHandlerTests()
        {
            _openingHoursRepositoryMock = new Mock<IOpeningHoursRepository>();
            _handler = new UpdateOpeningHourCommandHandler(_openingHoursRepositoryMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldUpdateOpeningHour_WhenCommandIsValid()
        {
            // Arrange
            var command = new UpdateOpeningHourCommand
            {
                Id = 1,
                DayOfWeek = "Wtorek",
                OpeningTime = TimeSpan.FromHours(10),
                ClosingTime = TimeSpan.FromHours(18),
                Closed = true,
                ServiceId = 2
            };

            var existingOpeningHour = new OpeningHour
            {
                Id = 1,
                DayOfWeek = "Poniedziałek",
                OpeningTime = TimeSpan.FromHours(8),
                ClosingTime = TimeSpan.FromHours(16),
                Closed = false,
                ServiceId = 1
            };

            _openingHoursRepositoryMock.Setup(x => x.GetByIdAsync(command.Id))
                .ReturnsAsync(existingOpeningHour);

            _openingHoursRepositoryMock.Setup(x => x.UpdateAsync(existingOpeningHour))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.Equal(Unit.Value, result);

            _openingHoursRepositoryMock.Verify(x => x.UpdateAsync(It.Is<OpeningHour>(o =>
                o.Id == command.Id &&
                o.DayOfWeek == command.DayOfWeek &&
                o.OpeningTime == command.OpeningTime &&
                o.ClosingTime == command.ClosingTime &&
                o.Closed == command.Closed &&
                o.ServiceId == command.ServiceId)), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldThrowKeyNotFoundException_WhenOpeningHourDoesNotExist()
        {
            // Arrange
            var command = new UpdateOpeningHourCommand { Id = 1 };

            _openingHoursRepositoryMock.Setup(x => x.GetByIdAsync(command.Id))
                .ReturnsAsync((OpeningHour)null);

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() => _handler.Handle(command, CancellationToken.None));
        }
    }
}
