using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using BookMe.Application.Employee.Commands.DeleteEmployee;
using BookMe.Domain.Entities;
using BookMe.Domain.Interfaces;
using MediatR;
using Moq;
using Xunit;

namespace BookMe.Application.Employee.Commands.DeleteEmployee.Tests
{
    public class DeleteEmployeeCommandHandlerTests
    {
        private readonly Mock<IEmployeeRepository> _employeeRepositoryMock;
        private readonly DeleteEmployeeCommandHandler _handler;

        public DeleteEmployeeCommandHandlerTests()
        {
            _employeeRepositoryMock = new Mock<IEmployeeRepository>();
            _handler = new DeleteEmployeeCommandHandler(_employeeRepositoryMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldDeleteEmployee_WhenEmployeeExists()
        {
            // Arrange
            var command = new DeleteEmployeeCommand { Id = 1 };

            var employee = new Domain.Entities.Employee { Id = 1 };

            _employeeRepositoryMock.Setup(x => x.GetEmployeeByIdAsync(command.Id))
                .ReturnsAsync(employee);

            _employeeRepositoryMock.Setup(x => x.DeleteAsync(command.Id))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.Equal(Unit.Value, result);
            _employeeRepositoryMock.Verify(x => x.DeleteAsync(command.Id), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldThrowKeyNotFoundException_WhenEmployeeDoesNotExist()
        {
            // Arrange
            var command = new DeleteEmployeeCommand { Id = 1 };

            _employeeRepositoryMock.Setup(x => x.GetEmployeeByIdAsync(command.Id))
                .ReturnsAsync((Domain.Entities.Employee)null);

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() => _handler.Handle(command, CancellationToken.None));
        }
    }
}
