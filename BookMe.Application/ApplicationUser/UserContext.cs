using BookMe.Application.ApplicationUser;
using BookMe.Domain.Entities;
using BookMe.Domain.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BookMe.Application.ApplicationUser
{
    public interface IUserContext
    {
        Task<CurrentUser> GetCurrentUserAsync();
        Task<bool> IsEmployeeAsync();
        Task<Domain.Entities.Employee> GetEmployeeByUserIdAsync(string userId);
    }

    public class UserContext : IUserContext
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<Domain.Entities.ApplicationUser> _userManager;
        private readonly IEmployeeRepository _employeeRepository;

        public UserContext(IHttpContextAccessor httpContextAccessor, UserManager<Domain.Entities.ApplicationUser> userManager, IEmployeeRepository employeeRepository)
        {
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
            _employeeRepository = employeeRepository;
        }

        public async Task<CurrentUser> GetCurrentUserAsync()
        {
            var user = _httpContextAccessor.HttpContext?.User;
            if (user == null)
            {
                throw new InvalidOperationException("Context user is not present");
            }

            if (user.Identity == null || !user.Identity.IsAuthenticated)
            {
                return null;
            }

            var id = user.FindFirst(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            var email = user.FindFirst(c => c.Type == ClaimTypes.Email)?.Value;
            var firstName = user.FindFirst(c => c.Type == "FirstName")?.Value;
            var lastName = user.FindFirst(c => c.Type == "LastName")?.Value;

            var applicationUser = await _userManager.FindByIdAsync(id);
            var isAdmin = applicationUser?.IsAdmin ?? false;

            return new CurrentUser(id, email, firstName, lastName, isAdmin);
        }

        public async Task<bool> IsEmployeeAsync()
        {
            var currentUser = await GetCurrentUserAsync();
            if (currentUser == null)
            {
                return false;
            }

            var employee = await _employeeRepository.GetEmployeeByUserIdAsync(currentUser.Id);
            return employee != null;
        }

        public async Task<Domain.Entities.Employee> GetEmployeeByUserIdAsync(string userId)
        {
            return await _employeeRepository.GetEmployeeByUserIdAsync(userId);
        }
    }
}
