using System.Threading;
using System.Threading.Tasks;
using BookMe.Application.Employee.Commands.CreateEmployee;
using BookMe.Domain.Entities;
using BookMe.Domain.Interfaces;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Moq;
using Xunit;

namespace BookMe.Application.Employee.Commands.CreateEmployee.Tests
{
    public class CreateEmployeeCommandHandlerTests
    {
        private readonly Mock<IEmployeeRepository> _employeeRepositoryMock;
        private readonly Mock<UserManager<Domain.Entities.ApplicationUser>> _userManagerMock;
        private readonly CreateEmployeeCommandHandler _handler;

        public CreateEmployeeCommandHandlerTests()
        {
            _employeeRepositoryMock = new Mock<IEmployeeRepository>();
            _userManagerMock = MockUserManager();
            _handler = new CreateEmployeeCommandHandler(_employeeRepositoryMock.Object, _userManagerMock.Object, null);
        }

        [Fact]
        public async Task Handle_ShouldCreateEmployee_WhenCommandIsValid()
        {
            // Arrange
            var command = new CreateEmployeeCommand
            {
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@example.com",
                Password = "Password123",
                ServiceId = 1
            };

            _userManagerMock.Setup(x => x.CreateAsync(It.IsAny<Domain.Entities.ApplicationUser>(), command.Password))
                .ReturnsAsync(IdentityResult.Success);

            _employeeRepositoryMock.Setup(x => x.CreateAsync(It.IsAny<Domain.Entities.Employee>()))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.Equal(Unit.Value, result);
            _userManagerMock.Verify(x => x.CreateAsync(It.Is<Domain.Entities.ApplicationUser>(u =>
                u.FirstName == command.FirstName &&
                u.LastName == command.LastName &&
                u.Email == command.Email), command.Password), Times.Once);

            _employeeRepositoryMock.Verify(x => x.CreateAsync(It.Is<Domain.Entities.Employee>(e =>
                e.ServiceId == command.ServiceId)), Times.Once);
        }

        private static Mock<UserManager<Domain.Entities.ApplicationUser>> MockUserManager()
        {
            var store = new Mock<IUserStore<Domain.Entities.ApplicationUser>>();
            return new Mock<UserManager<Domain.Entities.ApplicationUser>>(store.Object, null, null, null, null, null, null, null, null);
        }
    }
}
