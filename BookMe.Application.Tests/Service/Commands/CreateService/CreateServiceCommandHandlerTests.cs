using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using BookMe.Application.Service.Commands.CreateService;
using BookMe.Domain.Entities;
using BookMe.Domain.Interfaces;
using FluentValidation;
using MediatR;
using Moq;
using Xunit;

namespace BookMe.Application.Service.Commands.CreateService.Tests
{
    public class CreateServiceCommandHandlerTests
    {
        private readonly Mock<IServiceRepository> _serviceRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly CreateServiceCommandHandler _handler;

        public CreateServiceCommandHandlerTests()
        {
            _serviceRepositoryMock = new Mock<IServiceRepository>();
            _mapperMock = new Mock<IMapper>();
            _handler = new CreateServiceCommandHandler(_serviceRepositoryMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldAddService_WhenCommandIsValid()
        {
            // Arrange
            var command = new CreateServiceCommand
            {
                Name = "ServiceName",
                Description = "ServiceDescription",
                ServiceCategoryId = 1,
                City = "CityName",
                Street = "StreetName",
                PostalCode = "00-000",
                PhoneNumber = "+123456789"
            };

            _serviceRepositoryMock.Setup(x => x.GetByNameAsync(command.Name))
                .ReturnsAsync((Domain.Entities.Service)null);

            var service = new Domain.Entities.Service
            {
                Name = command.Name 
            };

            _mapperMock.Setup(m => m.Map<Domain.Entities.Service>(command))
                .Returns(service);

            _serviceRepositoryMock.Setup(x => x.AddAsync(service))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.Equal(Unit.Value, result);

            _serviceRepositoryMock.Verify(x => x.AddAsync(service), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldThrowValidationException_WhenServiceNameAlreadyExists()
        {
            // Arrange
            var command = new CreateServiceCommand { Name = "ExistingName" };
            _serviceRepositoryMock.Setup(x => x.GetByNameAsync(command.Name))
                .ReturnsAsync(new Domain.Entities.Service());

            // Act & Assert
            await Assert.ThrowsAsync<ValidationException>(() => _handler.Handle(command, CancellationToken.None));
        }
    }
}
