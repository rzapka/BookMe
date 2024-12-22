using BookMe.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BookMe.Domain.Interfaces
{
    public interface IOpinionRepository
    {
        Task CreateOpinionAsync(Opinion opinion);
        Task UpdateOpinionAsync(Opinion opinion);
        Task<Opinion> GetOpinionByIdAsync(int id);
        Task DeleteOpinionAsync(int id);
        Task<IEnumerable<Opinion>> GetOpinionsByServiceIdAsync(int serviceId);
        Task DeleteAsync(int opinionId);
        Task<IEnumerable<Opinion>> GetOpinionsByEmployeeIdAsync(int employeeId);

        Task<Service?> GetServiceByOpinionId(int opinionId);
        Task Commit();
        public Task<IEnumerable<Opinion>> GetOpinionsByServiceEncodedNameAsync(string encodedName);
    }
}
