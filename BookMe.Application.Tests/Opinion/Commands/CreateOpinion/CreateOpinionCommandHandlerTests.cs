using System.Threading;
using System.Threading.Tasks;
using BookMe.Application.Opinion.Commands.CreateOpinion;
using BookMe.Domain.Entities;
using BookMe.Domain.Interfaces;
using MediatR;
using Moq;
using Xunit;

namespace BookMe.Application.Opinion.Commands.CreateOpinion.Tests
{
    public class CreateOpinionCommandHandlerTests
    {
        private readonly Mock<IOpinionRepository> _opinionRepositoryMock;
        private readonly CreateOpinionCommandHandler _handler;

        public CreateOpinionCommandHandlerTests()
        {
            _opinionRepositoryMock = new Mock<IOpinionRepository>();
            _handler = new CreateOpinionCommandHandler(_opinionRepositoryMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldCreateOpinion_WhenCommandIsValid()
        {
            // Arrange
            var command = new CreateOpinionCommand
            {
                ServiceId = 1,
                EmployeeId = 2,
                UserId = "User123",
                BookingId = 3,
                Rating = 5,
                Content = "Great service!"
            };

            _opinionRepositoryMock.Setup(x => x.CreateOpinionAsync(It.IsAny<Domain.Entities.Opinion>()))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.Equal(Unit.Value, result);

            _opinionRepositoryMock.Verify(x => x.CreateOpinionAsync(It.Is<Domain.Entities.Opinion>(o =>
                o.ServiceId == command.ServiceId &&
                o.EmployeeId == command.EmployeeId &&
                o.UserId == command.UserId &&
                o.Rating == command.Rating &&
                o.Content == command.Content)), Times.Once);
        }
    }
}
