using System.Threading;
using System.Threading.Tasks;
using BookMe.Application.Employee.Commands.UpdateEmployee;
using BookMe.Domain.Entities;
using BookMe.Domain.Interfaces;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Moq;
using Xunit;

namespace BookMe.Application.Employee.Commands.UpdateEmployee.Tests
{
    public class UpdateEmployeeCommandHandlerTests
    {
        private readonly Mock<IEmployeeRepository> _employeeRepositoryMock;
        private readonly Mock<UserManager<Domain.Entities.ApplicationUser>> _userManagerMock;
        private readonly UpdateEmployeeCommandHandler _handler;

        public UpdateEmployeeCommandHandlerTests()
        {
            _employeeRepositoryMock = new Mock<IEmployeeRepository>();
            _userManagerMock = MockUserManager();
            _handler = new UpdateEmployeeCommandHandler(_employeeRepositoryMock.Object, _userManagerMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldUpdateEmployee_WhenCommandIsValid()
        {
            // Arrange
            var command = new UpdateEmployeeCommand
            {
                Id = 1,
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@example.com",
                ServiceId = 1
            };

            var employee = new Domain.Entities.Employee
            {
                Id = 1,
                User = new Domain.Entities.ApplicationUser { Id = "userId" }
            };

            _employeeRepositoryMock.Setup(x => x.GetEmployeeByIdAsync(command.Id))
                .ReturnsAsync(employee);

            _employeeRepositoryMock.Setup(x => x.UpdateAsync(employee))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.Equal(Unit.Value, result);
            _employeeRepositoryMock.Verify(x => x.UpdateAsync(It.Is<Domain.Entities.Employee>(e =>
                e.Id == command.Id &&
                e.User.FirstName == command.FirstName &&
                e.User.LastName == command.LastName &&
                e.User.Email == command.Email &&
                e.ServiceId == command.ServiceId)), Times.Once);
        }

        private static Mock<UserManager<Domain.Entities.ApplicationUser>> MockUserManager()
        {
            var store = new Mock<IUserStore<Domain.Entities.ApplicationUser>>();
            return new Mock<UserManager<Domain.Entities.ApplicationUser>>(store.Object, null, null, null, null, null, null, null, null);
        }
    }
}
