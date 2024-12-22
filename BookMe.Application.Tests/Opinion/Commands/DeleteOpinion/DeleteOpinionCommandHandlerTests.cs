using System.Threading;
using System.Threading.Tasks;
using BookMe.Application.Opinion.Commands.DeleteOpinion;
using BookMe.Domain.Interfaces;
using MediatR;
using Moq;
using Xunit;

namespace BookMe.Application.Opinion.Commands.DeleteOpinion.Tests
{
    public class DeleteOpinionCommandHandlerTests
    {
        private readonly Mock<IOpinionRepository> _opinionRepositoryMock;
        private readonly DeleteOpinionCommandHandler _handler;

        public DeleteOpinionCommandHandlerTests()
        {
            _opinionRepositoryMock = new Mock<IOpinionRepository>();
            _handler = new DeleteOpinionCommandHandler(_opinionRepositoryMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldDeleteOpinion_WhenCommandIsValid()
        {
            // Arrange
            var command = new DeleteOpinionCommand { Id = 1 };

            _opinionRepositoryMock.Setup(x => x.DeleteOpinionAsync(command.Id))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.Equal(Unit.Value, result);

            _opinionRepositoryMock.Verify(x => x.DeleteOpinionAsync(command.Id), Times.Once);
        }
    }
}
