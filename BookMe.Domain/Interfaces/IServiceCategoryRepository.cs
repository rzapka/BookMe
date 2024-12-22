using BookMe.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BookMe.Domain.Interfaces
{
    public interface IServiceCategoryRepository
    {
        Task<IEnumerable<ServiceCategory>> GetAll();
        Task<ServiceCategory?> GetByEncodedName(string encodedName, string searchTerm = "");
        Task<ServiceCategory?> GetByIdAsync(int id);
        Task AddAsync(ServiceCategory category);
        Task UpdateAsync(ServiceCategory category);
        Task DeleteAsync(ServiceCategory category);

        public Task<IEnumerable<ServiceCategory>> SearchAsync(string searchTerm);
    }
}
