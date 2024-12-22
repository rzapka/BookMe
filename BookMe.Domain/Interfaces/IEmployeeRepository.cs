using BookMe.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BookMe.Domain.Interfaces
{
    public interface IEmployeeRepository
    {
        Task CreateAsync(Employee employee);
        Task<Employee> GetEmployeeByIdAsync(int employeeId);
        Task<Employee> GetEmployeeByUserIdAsync(string userId);
        Task UpdateAsync(Employee employee);
        Task DeleteAsync(int employeeId);
        Task<IEnumerable<Employee>> GetEmployeesByServiceIdAsync(int serviceId);
        Task<IEnumerable<Employee>> GetEmployeesByServiceEncodedNameAsync(string serviceEncodedName);
        Task<IEnumerable<Employee>> GetAllEmployeesAsync();
        Task<IEnumerable<Employee>> SearchEmployeesAsync(string searchTerm);

        Task<Dictionary<int, string>> GetEmployeesByServiceEncodedNameAsDictionary(string serviceEncodedName);
    }
}
