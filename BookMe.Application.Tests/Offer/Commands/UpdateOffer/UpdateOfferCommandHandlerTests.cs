using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using BookMe.Application.Offer.Commands.UpdateOffer;
using BookMe.Domain.Entities;
using BookMe.Domain.Interfaces;
using MediatR;
using Moq;
using Xunit;

namespace BookMe.Application.Offer.Commands.UpdateOffer.Tests
{
    public class UpdateOfferCommandHandlerTests
    {
        private readonly Mock<IOfferRepository> _offerRepositoryMock;
        private readonly UpdateOfferCommandHandler _handler;

        public UpdateOfferCommandHandlerTests()
        {
            _offerRepositoryMock = new Mock<IOfferRepository>();
            _handler = new UpdateOfferCommandHandler(_offerRepositoryMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldUpdateOffer_WhenCommandIsValid()
        {
            // Arrange
            var command = new UpdateOfferCommand
            {
                Id = 1,
                Name = "UpdatedOffer",
                Duration = 120,
                Price = 199.99m
            };

            var existingOffer = new Domain.Entities.Offer
            {
                Id = 1,
                Name = "OldOffer",
                Duration = 60,
                Price = 99.99m
            };

            _offerRepositoryMock.Setup(x => x.GetByIdAsync(command.Id))
                .ReturnsAsync(existingOffer);

            _offerRepositoryMock.Setup(x => x.UpdateAsync(existingOffer))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.Equal(Unit.Value, result);

            _offerRepositoryMock.Verify(x => x.UpdateAsync(It.Is<Domain.Entities.Offer>(o =>
                o.Id == command.Id &&
                o.Name == command.Name &&
                o.Duration == command.Duration &&
                o.Price == command.Price)), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldThrowKeyNotFoundException_WhenOfferDoesNotExist()
        {
            // Arrange
            var command = new UpdateOfferCommand { Id = 1 };

            _offerRepositoryMock.Setup(x => x.GetByIdAsync(command.Id))
                .ReturnsAsync((Domain.Entities.Offer)null);

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() => _handler.Handle(command, CancellationToken.None));
        }
    }
}
