using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using BookMe.Application.ServiceCategory.Commands.DeleteServiceCategory;
using BookMe.Domain.Entities;
using BookMe.Domain.Interfaces;
using MediatR;
using Moq;
using Xunit;

namespace BookMe.Application.ServiceCategory.Commands.DeleteServiceCategory.Tests
{
    public class DeleteServiceCategoryCommandHandlerTests
    {
        private readonly Mock<IServiceCategoryRepository> _serviceCategoryRepositoryMock;
        private readonly DeleteServiceCategoryCommandHandler _handler;

        public DeleteServiceCategoryCommandHandlerTests()
        {
            _serviceCategoryRepositoryMock = new Mock<IServiceCategoryRepository>();
            _handler = new DeleteServiceCategoryCommandHandler(_serviceCategoryRepositoryMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldDeleteServiceCategory_WhenCommandIsValid()
        {
            // Arrange
            var command = new DeleteServiceCategoryCommand { Id = 1 };

            var existingCategory = new Domain.Entities.ServiceCategory
            {
                Id = 1,
                Name = "CategoryName"
            };

            _serviceCategoryRepositoryMock.Setup(x => x.GetByIdAsync(command.Id))
                .ReturnsAsync(existingCategory);

            _serviceCategoryRepositoryMock.Setup(x => x.DeleteAsync(existingCategory))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.Equal(Unit.Value, result);

            _serviceCategoryRepositoryMock.Verify(x => x.GetByIdAsync(command.Id), Times.Once);
            _serviceCategoryRepositoryMock.Verify(x => x.DeleteAsync(existingCategory), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldThrowKeyNotFoundException_WhenServiceCategoryDoesNotExist()
        {
            // Arrange
            var command = new DeleteServiceCategoryCommand { Id = 1 };

            _serviceCategoryRepositoryMock.Setup(x => x.GetByIdAsync(command.Id))
                .ReturnsAsync((Domain.Entities.ServiceCategory)null);

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() => _handler.Handle(command, CancellationToken.None));
        }
    }
}
