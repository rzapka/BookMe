using System.Threading;
using System.Threading.Tasks;
using BookMe.Application.ServiceCategory.Commands.CreateServiceCategory;
using BookMe.Domain.Entities;
using BookMe.Domain.Interfaces;
using MediatR;
using Moq;
using Xunit;

namespace BookMe.Application.ServiceCategory.Commands.CreateServiceCategory.Tests
{
    public class CreateServiceCategoryCommandHandlerTests
    {
        private readonly Mock<IServiceCategoryRepository> _serviceCategoryRepositoryMock;
        private readonly CreateServiceCategoryCommandHandler _handler;

        public CreateServiceCategoryCommandHandlerTests()
        {
            _serviceCategoryRepositoryMock = new Mock<IServiceCategoryRepository>();
            _handler = new CreateServiceCategoryCommandHandler(_serviceCategoryRepositoryMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldAddServiceCategory_WhenCommandIsValid()
        {
            // Arrange
            var command = new CreateServiceCategoryCommand
            {
                Name = "CategoryName",
                ImageUrl = "https://example.com/image.jpg"
            };

            _serviceCategoryRepositoryMock.Setup(x => x.AddAsync(It.IsAny<Domain.Entities.ServiceCategory>()))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.Equal(Unit.Value, result);

            _serviceCategoryRepositoryMock.Verify(x => x.AddAsync(It.Is<Domain.Entities.ServiceCategory>(c =>
                c.Name == command.Name &&
                c.ImageUrl == command.ImageUrl)), Times.Once);
        }
    }
}
