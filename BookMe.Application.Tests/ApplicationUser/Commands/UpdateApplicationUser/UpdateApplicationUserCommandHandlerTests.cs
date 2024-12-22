using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BookMe.Application.ApplicationUser.Commands.UpdateApplicationUser;
using BookMe.Application.Exceptions;
using BookMe.Domain.Entities;
using BookMe.Domain.Interfaces;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Moq;
using Xunit;

namespace BookMe.Application.ApplicationUser.Commands.UpdateApplicationUser.Tests
{
    public class UpdateApplicationUserCommandHandlerTests
    {
        private readonly Mock<IApplicationUserRepository> _userRepositoryMock;
        private readonly Mock<IEmployeeRepository> _employeeRepositoryMock;
        private readonly Mock<UserManager<Domain.Entities.ApplicationUser>> _userManagerMock;
        private readonly UpdateApplicationUserCommandHandler _handler;

        public UpdateApplicationUserCommandHandlerTests()
        {
            _userRepositoryMock = new Mock<IApplicationUserRepository>();
            _employeeRepositoryMock = new Mock<IEmployeeRepository>();
            _userManagerMock = MockUserManager<Domain.Entities.ApplicationUser>(); // Określenie typu

            _handler = new UpdateApplicationUserCommandHandler(
                _userRepositoryMock.Object,
                _employeeRepositoryMock.Object,
                _userManagerMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldUpdateUser_WhenCommandIsValid()
        {
            // Arrange
            var existingUser = new Domain.Entities.ApplicationUser { Id = "123", Email = "old@example.com" };
            var command = new UpdateApplicationUserCommand
            {
                Id = "123",
                FirstName = "John",
                LastName = "Doe",
                Email = "new@example.com",
                NewPassword = "NewPassword123",
                ServiceId = 42
            };

            _userRepositoryMock.Setup(x => x.GetByIdAsync(command.Id))
                .ReturnsAsync(existingUser);

            _userManagerMock.Setup(x => x.FindByEmailAsync(command.Email))
                .ReturnsAsync((Domain.Entities.ApplicationUser)null);

            _userManagerMock.Setup(x => x.GeneratePasswordResetTokenAsync(existingUser))
                .ReturnsAsync("reset-token");

            _userManagerMock.Setup(x => x.ResetPasswordAsync(existingUser, "reset-token", command.NewPassword))
                .ReturnsAsync(IdentityResult.Success);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.Equal(Unit.Value, result);

            _userRepositoryMock.Verify(x => x.UpdateAsync(It.Is<Domain.Entities.ApplicationUser>(u =>
                u.Id == command.Id &&
                u.Email == command.Email &&
                u.FirstName == command.FirstName &&
                u.LastName == command.LastName)), Times.Once);

            _employeeRepositoryMock.Verify(x => x.CreateAsync(It.Is<Domain.Entities.Employee>(e =>
                e.UserId == command.Id &&
                e.ServiceId == command.ServiceId)), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldThrowException_WhenUserNotFound()
        {
            // Arrange
            var command = new UpdateApplicationUserCommand { Id = "123" };

            _userRepositoryMock.Setup(x => x.GetByIdAsync(command.Id))
                .ReturnsAsync((Domain.Entities.ApplicationUser)null);

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() => _handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_ShouldThrowException_WhenEmailAlreadyExists()
        {
            // Arrange
            var existingUser = new Domain.Entities.ApplicationUser { Id = "123", Email = "old@example.com" };
            var command = new UpdateApplicationUserCommand { Id = "123", Email = "existing@example.com" };

            _userRepositoryMock.Setup(x => x.GetByIdAsync(command.Id))
                .ReturnsAsync(existingUser);

            _userManagerMock.Setup(x => x.FindByEmailAsync(command.Email))
                .ReturnsAsync(new Domain.Entities.ApplicationUser { Id = "456" });

            // Act & Assert
            await Assert.ThrowsAsync<UserEmailConflictException>(() => _handler.Handle(command, CancellationToken.None));
        }

        private static Mock<UserManager<TUser>> MockUserManager<TUser>() where TUser : class
        {
            var store = new Mock<IUserStore<TUser>>();
            return new Mock<UserManager<TUser>>(store.Object, null, null, null, null, null, null, null, null);
        }
    }
}
