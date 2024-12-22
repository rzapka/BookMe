using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using BookMe.Application.OpeningHours.Commands.DeleteOpeningHour;
using BookMe.Domain.Entities;
using BookMe.Domain.Interfaces;
using MediatR;
using Moq;
using Xunit;

namespace BookMe.Application.OpeningHours.Commands.DeleteOpeningHour.Tests
{
    public class DeleteOpeningHourCommandHandlerTests
    {
        private readonly Mock<IOpeningHoursRepository> _openingHoursRepositoryMock;
        private readonly DeleteOpeningHourCommandHandler _handler;

        public DeleteOpeningHourCommandHandlerTests()
        {
            _openingHoursRepositoryMock = new Mock<IOpeningHoursRepository>();
            _handler = new DeleteOpeningHourCommandHandler(_openingHoursRepositoryMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldDeleteOpeningHour_WhenCommandIsValid()
        {
            // Arrange
            var command = new DeleteOpeningHourCommand { Id = 1 };

            var existingOpeningHour = new OpeningHour
            {
                Id = 1,
                DayOfWeek = "Środa",
                OpeningTime = TimeSpan.FromHours(8),
                ClosingTime = TimeSpan.FromHours(16),
                Closed = false,
                ServiceId = 1
            };

            _openingHoursRepositoryMock.Setup(x => x.GetByIdAsync(command.Id))
                .ReturnsAsync(existingOpeningHour);

            _openingHoursRepositoryMock.Setup(x => x.DeleteAsync(command.Id))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.Equal(Unit.Value, result);

            _openingHoursRepositoryMock.Verify(x => x.DeleteAsync(command.Id), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldThrowKeyNotFoundException_WhenOpeningHourDoesNotExist()
        {
            // Arrange
            var command = new DeleteOpeningHourCommand { Id = 1 };

            _openingHoursRepositoryMock.Setup(x => x.GetByIdAsync(command.Id))
                .ReturnsAsync((OpeningHour)null);

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() => _handler.Handle(command, CancellationToken.None));
        }
    }
}
