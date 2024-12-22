using System;
using System.Threading;
using System.Threading.Tasks;
using BookMe.Application.OpeningHours.Commands.CreateOpeningHour;
using BookMe.Domain.Entities;
using BookMe.Domain.Interfaces;
using MediatR;
using Moq;
using Xunit;

namespace BookMe.Application.OpeningHours.Commands.CreateOpeningHour.Tests
{
    public class CreateOpeningHourCommandHandlerTests
    {
        private readonly Mock<IOpeningHoursRepository> _openingHoursRepositoryMock;
        private readonly CreateOpeningHourCommandHandler _handler;

        public CreateOpeningHourCommandHandlerTests()
        {
            _openingHoursRepositoryMock = new Mock<IOpeningHoursRepository>();
            _handler = new CreateOpeningHourCommandHandler(_openingHoursRepositoryMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldAddOpeningHour_WhenCommandIsValid()
        {
            // Arrange
            var command = new CreateOpeningHourCommand
            {
                DayOfWeek = "Poniedziałek",
                OpeningTime = TimeSpan.FromHours(8),
                ClosingTime = TimeSpan.FromHours(16),
                Closed = false,
                ServiceId = 1
            };

            _openingHoursRepositoryMock.Setup(x => x.AddAsync(It.IsAny<OpeningHour>()))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.Equal(Unit.Value, result);

            _openingHoursRepositoryMock.Verify(x => x.AddAsync(It.Is<OpeningHour>(o =>
                o.DayOfWeek == command.DayOfWeek &&
                o.OpeningTime == command.OpeningTime &&
                o.ClosingTime == command.ClosingTime &&
                o.Closed == command.Closed &&
                o.ServiceId == command.ServiceId)), Times.Once);
        }
    }
}
