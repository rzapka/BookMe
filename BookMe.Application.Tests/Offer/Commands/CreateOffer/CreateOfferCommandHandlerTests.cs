using System.Threading;
using System.Threading.Tasks;
using BookMe.Application.Offer.Commands.CreateOffer;
using BookMe.Domain.Entities;
using BookMe.Domain.Interfaces;
using MediatR;
using Moq;
using Xunit;

namespace BookMe.Application.Offer.Commands.CreateOffer.Tests
{
    public class CreateOfferCommandHandlerTests
    {
        private readonly Mock<IOfferRepository> _offerRepositoryMock;
        private readonly CreateOfferCommandHandler _handler;

        public CreateOfferCommandHandlerTests()
        {
            _offerRepositoryMock = new Mock<IOfferRepository>();
            _handler = new CreateOfferCommandHandler(_offerRepositoryMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldCreateOffer_WhenCommandIsValid()
        {
            // Arrange
            var command = new CreateOfferCommand
            {
                Name = "OfferName",
                Duration = 60,
                Price = 99.99m,
                ServiceId = 1
            };

            _offerRepositoryMock.Setup(x => x.AddAsync(It.IsAny<Domain.Entities.Offer>()))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.Equal(Unit.Value, result);

            _offerRepositoryMock.Verify(x => x.AddAsync(It.Is<Domain.Entities.Offer>(o =>
                o.Name == command.Name &&
                o.Duration == command.Duration &&
                o.Price == command.Price &&
                o.ServiceId == command.ServiceId)), Times.Once);
        }
    }
}
