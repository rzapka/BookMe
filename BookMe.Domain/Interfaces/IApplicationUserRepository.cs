using BookMe.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BookMe.Domain.Interfaces
{
    public interface IApplicationUserRepository
    {
        Task<ApplicationUser?> GetByIdAsync(string id);
        Task<IEnumerable<ApplicationUser>> GetAllAsync();
        Task<IEnumerable<ApplicationUser>> SearchAsync(string searchTerm);
        Task<IEnumerable<ApplicationUser>> GetUsersWithoutEmployeesAsync(); // Dodana metoda
        Task UpdateAsync(ApplicationUser user);
        Task DeleteAsync(string id);
    }
}
