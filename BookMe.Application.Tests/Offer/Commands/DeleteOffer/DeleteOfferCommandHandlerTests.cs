using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using BookMe.Application.Offer.Commands.DeleteOffer;
using BookMe.Domain.Entities;
using BookMe.Domain.Interfaces;
using MediatR;
using Moq;
using Xunit;

namespace BookMe.Application.Offer.Commands.DeleteOffer.Tests
{
    public class DeleteOfferCommandHandlerTests
    {
        private readonly Mock<IOfferRepository> _offerRepositoryMock;
        private readonly DeleteOfferCommandHandler _handler;

        public DeleteOfferCommandHandlerTests()
        {
            _offerRepositoryMock = new Mock<IOfferRepository>();
            _handler = new DeleteOfferCommandHandler(_offerRepositoryMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldDeleteOffer_WhenCommandIsValid()
        {
            // Arrange
            var command = new DeleteOfferCommand { Id = 1 };

            var existingOffer = new Domain.Entities.Offer { Id = 1 };

            _offerRepositoryMock.Setup(x => x.GetByIdAsync(command.Id))
                .ReturnsAsync(existingOffer);

            _offerRepositoryMock.Setup(x => x.DeleteAsync(command.Id))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.Equal(Unit.Value, result);

            _offerRepositoryMock.Verify(x => x.DeleteAsync(command.Id), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldThrowKeyNotFoundException_WhenOfferDoesNotExist()
        {
            // Arrange
            var command = new DeleteOfferCommand { Id = 1 };

            _offerRepositoryMock.Setup(x => x.GetByIdAsync(command.Id))
                .ReturnsAsync((Domain.Entities.Offer)null);

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() => _handler.Handle(command, CancellationToken.None));
        }
    }
}
