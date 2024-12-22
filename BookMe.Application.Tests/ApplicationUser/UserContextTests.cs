using Xunit;
using BookMe.Application.ApplicationUser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookMe.Domain.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Moq;
using System.Security.Claims;

namespace BookMe.Application.ApplicationUser.Tests
{
    public class UserContextTests
    {
        private readonly Mock<IHttpContextAccessor> _httpContextAccessorMock;
        private readonly Mock<UserManager<Domain.Entities.ApplicationUser>> _userManagerMock;
        private readonly Mock<IEmployeeRepository> _employeeRepositoryMock;

        private readonly UserContext _userContext;

        public UserContextTests()
        {
            _httpContextAccessorMock = new Mock<IHttpContextAccessor>();
            _userManagerMock = MockUserManager();
            _employeeRepositoryMock = new Mock<IEmployeeRepository>();

            _userContext = new UserContext(
                _httpContextAccessorMock.Object,
                _userManagerMock.Object,
                _employeeRepositoryMock.Object);
        }

        [Fact]
        public async Task GetCurrentUserAsync_ShouldReturnCurrentUser_WhenUserIsAuthenticated()
        {
            // Arrange
            var claims = new[]
            {
            new Claim(ClaimTypes.NameIdentifier, "123"),
            new Claim(ClaimTypes.Email, "user@example.com"),
            new Claim("FirstName", "John"),
            new Claim("LastName", "Doe")
        };

            var claimsIdentity = new ClaimsIdentity(claims, "TestAuth");
            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

            _httpContextAccessorMock.Setup(x => x.HttpContext).Returns(new DefaultHttpContext
            {
                User = claimsPrincipal
            });

            var applicationUser = new Domain.Entities.ApplicationUser { Id = "123", Email = "user@example.com", IsAdmin = true };
            _userManagerMock.Setup(x => x.FindByIdAsync("123")).ReturnsAsync(applicationUser);

            // Act
            var result = await _userContext.GetCurrentUserAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal("123", result.Id);
            Assert.Equal("user@example.com", result.Email);
            Assert.Equal("John", result.FirstName);
            Assert.Equal("Doe", result.LastName);
            Assert.True(result.IsAdmin);
        }

        [Fact]
        public async Task GetCurrentUserAsync_ShouldThrowInvalidOperationException_WhenUserIsNull()
        {
            // Arrange
            _httpContextAccessorMock.Setup(x => x.HttpContext).Returns((HttpContext)null);

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => _userContext.GetCurrentUserAsync());
        }

        private static Mock<UserManager<Domain.Entities.ApplicationUser>> MockUserManager()
        {
            var store = new Mock<IUserStore<Domain.Entities.ApplicationUser>>();
            return new Mock<UserManager<Domain.Entities.ApplicationUser>>(
                store.Object, null, null, null, null, null, null, null, null);
        }

        [Fact]
        public async Task IsEmployeeAsync_ShouldReturnTrue_WhenUserIsEmployee()
        {
            // Arrange
            var claims = new[]
            {
        new Claim(ClaimTypes.NameIdentifier, "123"),
        new Claim(ClaimTypes.Email, "user@example.com"),
        new Claim("FirstName", "John"),
        new Claim("LastName", "Doe")
    };

            var claimsIdentity = new ClaimsIdentity(claims, "TestAuth");
            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

            _httpContextAccessorMock.Setup(x => x.HttpContext).Returns(new DefaultHttpContext
            {
                User = claimsPrincipal
            });

            _userManagerMock.Setup(x => x.FindByIdAsync("123"))
                .ReturnsAsync(new Domain.Entities.ApplicationUser { Id = "123", Email = "user@example.com" });

            _employeeRepositoryMock.Setup(x => x.GetEmployeeByUserIdAsync("123"))
                .ReturnsAsync(new Domain.Entities.Employee { UserId = "123" });

            // Act
            var result = await _userContext.IsEmployeeAsync();

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task IsEmployeeAsync_ShouldReturnFalse_WhenUserIsNotEmployee()
        {
            // Arrange
            var currentUser = new CurrentUser("123", "user@example.com", "John", "Doe", false);
            _httpContextAccessorMock.Setup(x => x.HttpContext.User).Returns(new ClaimsPrincipal());

            _userManagerMock.Setup(x => x.FindByIdAsync("123")).ReturnsAsync(new Domain.Entities.ApplicationUser { Id = "123" });
            _employeeRepositoryMock.Setup(x => x.GetEmployeeByUserIdAsync("123"))
                .ReturnsAsync((Domain.Entities.Employee)null);

            // Act
            var result = await _userContext.IsEmployeeAsync();

            // Assert
            Assert.False(result);
        }
    }
}