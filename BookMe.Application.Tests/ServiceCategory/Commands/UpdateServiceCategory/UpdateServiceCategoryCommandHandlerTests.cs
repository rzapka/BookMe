using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using BookMe.Application.ServiceCategory.Commands.UpdateServiceCategory;
using BookMe.Domain.Entities;
using BookMe.Domain.Interfaces;
using MediatR;
using Moq;
using Xunit;

namespace BookMe.Application.ServiceCategory.Commands.UpdateServiceCategory.Tests
{
    public class UpdateServiceCategoryCommandHandlerTests
    {
        private readonly Mock<IServiceCategoryRepository> _serviceCategoryRepositoryMock;
        private readonly UpdateServiceCategoryCommandHandler _handler;

        public UpdateServiceCategoryCommandHandlerTests()
        {
            _serviceCategoryRepositoryMock = new Mock<IServiceCategoryRepository>();
            _handler = new UpdateServiceCategoryCommandHandler(_serviceCategoryRepositoryMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldUpdateServiceCategory_WhenCommandIsValid()
        {
            // Arrange
            var command = new UpdateServiceCategoryCommand
            {
                Id = 1,
                Name = "UpdatedName",
                ImageUrl = "https://example.com/updated-image.jpg"
            };

            var existingCategory = new Domain.Entities.ServiceCategory
            {
                Id = 1,
                Name = "OldName",
                ImageUrl = "https://example.com/old-image.jpg"
            };

            _serviceCategoryRepositoryMock.Setup(x => x.GetByIdAsync(command.Id))
                .ReturnsAsync(existingCategory);

            _serviceCategoryRepositoryMock.Setup(x => x.UpdateAsync(existingCategory))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.Equal(Unit.Value, result);

            _serviceCategoryRepositoryMock.Verify(x => x.GetByIdAsync(command.Id), Times.Once);
            _serviceCategoryRepositoryMock.Verify(x => x.UpdateAsync(It.Is<Domain.Entities.ServiceCategory>(c =>
                c.Name == command.Name && c.ImageUrl == command.ImageUrl)), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldThrowKeyNotFoundException_WhenServiceCategoryDoesNotExist()
        {
            // Arrange
            var command = new UpdateServiceCategoryCommand
            {
                Id = 1,
                Name = "CategoryName",
                ImageUrl = "https://example.com/image.jpg"
            };

            _serviceCategoryRepositoryMock.Setup(x => x.GetByIdAsync(command.Id))
                .ReturnsAsync((Domain.Entities.ServiceCategory)null);

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() => _handler.Handle(command, CancellationToken.None));
        }
    }
}
