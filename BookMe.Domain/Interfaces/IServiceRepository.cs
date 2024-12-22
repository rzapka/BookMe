using BookMe.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BookMe.Domain.Interfaces
{
    public interface IServiceRepository
    {
        Task<IEnumerable<Service>> GetServicesByCategoryEncodedName(string encodedName);
        Task<Service> GetServiceDetailsByEncodedName(string encodedName);
        Task<Service?> GetServiceByEncodedName(string encodedName);
        Task<Service> GetServiceByIdAsync(int serviceId);
        Task<List<Offer>> SearchOffersAsync(string term);
        Task<List<string>> SearchCitiesAsync(string term);
        Task<List<Service>> SearchServicesAsync(string searchTerm, string city);
        Task<List<Service>> GetRecommendedServicesAsync();

        Task<Service> GetFirstServiceWithOpeningHours();

        Task<Service?> GetByNameAsync(string name);
        Task DeleteAsync(Service service);
        Task<IEnumerable<Service>> GetAllAsync();
        Task<List<Service>> SearchServicesAsync(string searchTerm);
        Task AddAsync(Service service);
        Task Commit();
        Task<List<Employee>> GetEmployeesByServiceIdAsync(int serviceId);
        Task<List<Offer>> GetOffersByServiceIdAsync(int serviceId);
    }
}
