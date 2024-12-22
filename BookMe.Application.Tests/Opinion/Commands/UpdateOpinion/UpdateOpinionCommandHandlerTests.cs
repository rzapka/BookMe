using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using BookMe.Application.Opinion.Commands.UpdateOpinion;
using BookMe.Domain.Entities;
using BookMe.Domain.Interfaces;
using MediatR;
using Moq;
using Xunit;

namespace BookMe.Application.Opinion.Commands.UpdateOpinion.Tests
{
    public class UpdateOpinionCommandHandlerTests
    {
        private readonly Mock<IOpinionRepository> _opinionRepositoryMock;
        private readonly UpdateOpinionCommandHandler _handler;

        public UpdateOpinionCommandHandlerTests()
        {
            _opinionRepositoryMock = new Mock<IOpinionRepository>();
            _handler = new UpdateOpinionCommandHandler(_opinionRepositoryMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldUpdateOpinion_WhenCommandIsValid()
        {
            // Arrange
            var command = new UpdateOpinionCommand
            {
                Id = 1,
                Rating = 4,
                Content = "Good service",
                EmployeeId = 2,
                OfferId = 3
            };

            var existingOpinion = new Domain.Entities.Opinion { Id = 1, Rating = 5, Content = "Old content" };

            _opinionRepositoryMock.Setup(x => x.GetOpinionByIdAsync(command.Id))
                .ReturnsAsync(existingOpinion);

            _opinionRepositoryMock.Setup(x => x.Commit())
                .Returns(Task.CompletedTask);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.Equal(Unit.Value, result);

            _opinionRepositoryMock.Verify(x => x.Commit(), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldThrowKeyNotFoundException_WhenOpinionDoesNotExist()
        {
            // Arrange
            var command = new UpdateOpinionCommand { Id = 1 };

            _opinionRepositoryMock.Setup(x => x.GetOpinionByIdAsync(command.Id))
                .ReturnsAsync((Domain.Entities.Opinion)null);

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() => _handler.Handle(command, CancellationToken.None));
        }
    }
}
