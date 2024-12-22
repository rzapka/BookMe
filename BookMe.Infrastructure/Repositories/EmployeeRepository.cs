using BookMe.Domain.Entities;
using BookMe.Domain.Interfaces;
using BookMe.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookMe.Infrastructure.Repositories
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly BookMeDbContext _dbContext;

        public EmployeeRepository(BookMeDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task CreateAsync(Employee employee)
        {
            _dbContext.Employees.Add(employee);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<Employee> GetEmployeeByIdAsync(int employeeId)
        {
            return await _dbContext.Employees
                .Include(e => e.User)
                .Include(e => e.Service)
                .FirstOrDefaultAsync(e => e.Id == employeeId);
        }

        public async Task<Employee> GetEmployeeByUserIdAsync(string userId)
        {
            return await _dbContext.Employees
                .Include(e => e.User)
                .FirstOrDefaultAsync(e => e.UserId == userId);
        }

        public async Task UpdateAsync(Employee employee)
        {
            _dbContext.Employees.Update(employee);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(int employeeId)
        {
            var employee = await _dbContext.Employees.FindAsync(employeeId);
            if (employee != null)
            {
                _dbContext.Employees.Remove(employee);
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Employee>> GetEmployeesByServiceIdAsync(int serviceId)
        {
            return await _dbContext.Employees
                .Include(e => e.User)
                .Where(e => e.ServiceId == serviceId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Employee>> GetAllEmployeesAsync()
        {
            return await _dbContext.Employees
                .Include(e => e.User)
                .Include(e => e.Service)
                .ToListAsync();
        }

        public async Task<IEnumerable<Employee>> SearchEmployeesAsync(string searchTerm)
        {
            searchTerm = searchTerm.ToLower();
            return await _dbContext.Employees
                .Include(e => e.User)
                .Include(e => e.Service)
                .Where(e => e.User.FirstName.ToLower().Contains(searchTerm) || e.User.LastName.ToLower().Contains(searchTerm))
                .ToListAsync();
        }

        public async Task<IEnumerable<Employee>> GetEmployeesByServiceEncodedNameAsync(string serviceEncodedName)
        {
            return await _dbContext.Employees
                .Include(e => e.User)
                .Include(e => e.Service)
                .Where(e => e.Service.EncodedName == serviceEncodedName)
                .ToListAsync();
        }

        public async Task<Dictionary<int, string>> GetEmployeesByServiceEncodedNameAsDictionary(string serviceEncodedName)
        {
            return await _dbContext.Employees
                .Include(e => e.User) 
                .Include(e => e.Service)
                .Where(e => e.Service.EncodedName == serviceEncodedName)
                .ToDictionaryAsync(
                    e => e.Id,
                    e => e.User.FirstName + " " + e.User.LastName 
                );
        }
    }
}
