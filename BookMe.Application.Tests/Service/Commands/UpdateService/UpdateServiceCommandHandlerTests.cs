using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using BookMe.Application.Service.Commands.UpdateService;
using BookMe.Domain.Entities;
using BookMe.Domain.Interfaces;
using MediatR;
using Moq;
using Xunit;

namespace BookMe.Application.Service.Commands.UpdateService.Tests
{
    public class UpdateServiceCommandHandlerTests
    {
        private readonly Mock<IServiceRepository> _serviceRepositoryMock;
        private readonly UpdateServiceCommandHandler _handler;

        public UpdateServiceCommandHandlerTests()
        {
            _serviceRepositoryMock = new Mock<IServiceRepository>();
            _handler = new UpdateServiceCommandHandler(_serviceRepositoryMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldUpdateService_WhenCommandIsValid()
        {
            // Arrange
            var command = new UpdateServiceCommand
            {
                EncodedName = "service-encoded",
                Name = "UpdatedName",
                Description = "Updated Description",
                ServiceCategoryId = 1,
                City = "Test City",
                Street = "Test Street",
                PostalCode = "12-345",
                PhoneNumber = "+48123456789",
                ImageUrl = "http://example.com/image.jpg"
            };

            var existingService = new Domain.Entities.Service
            {
                Name = "OldName",
                Description = "Old Description",
                ServiceCategoryId = 1,
                ContactDetails = new ServiceContactDetails
                {
                    City = "Old City",
                    Street = "Old Street",
                    PostalCode = "11-111",
                    PhoneNumber = "+48000000000"
                },
                OpeningHours = new List<OpeningHour>()
            };

            _serviceRepositoryMock.Setup(x => x.GetServiceByEncodedName(command.EncodedName))
                .ReturnsAsync(existingService);

            _serviceRepositoryMock.Setup(x => x.Commit())
                .Returns(Task.CompletedTask);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.Equal(Unit.Value, result);

            _serviceRepositoryMock.Verify(x => x.Commit(), Times.Once);

            // Validate updated values
            Assert.Equal(command.Name, existingService.Name);
            Assert.Equal(command.Description, existingService.Description);
            Assert.Equal(command.ServiceCategoryId, existingService.ServiceCategoryId);
            Assert.Equal(command.City, existingService.ContactDetails.City);
            Assert.Equal(command.Street, existingService.ContactDetails.Street);
            Assert.Equal(command.PostalCode, existingService.ContactDetails.PostalCode);
            Assert.Equal(command.PhoneNumber, existingService.ContactDetails.PhoneNumber);
        }

        [Fact]
        public async Task Handle_ShouldThrowKeyNotFoundException_WhenServiceDoesNotExist()
        {
            // Arrange
            var command = new UpdateServiceCommand { EncodedName = "not-found" };

            _serviceRepositoryMock.Setup(x => x.GetServiceByEncodedName(command.EncodedName))
                .ReturnsAsync((Domain.Entities.Service)null);

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() => _handler.Handle(command, CancellationToken.None));
        }
    }
}
