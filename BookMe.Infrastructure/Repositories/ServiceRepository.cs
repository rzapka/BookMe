using BookMe.Domain.Entities;
using BookMe.Domain.Interfaces;
using BookMe.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace BookMe.Infrastructure.Repositories
{
    public class ServiceRepository : IServiceRepository
    {
        private readonly BookMeDbContext _dbContext;

        public ServiceRepository(BookMeDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<Service>> GetServicesByCategoryEncodedName(string encodedName)
            => await _dbContext.Services
                .Where(x => x.ServiceCategory.EncodedName == encodedName)
                .Include(s => s.ServiceCategory)
                .ToListAsync();

        public async Task<Service> GetServiceDetailsByEncodedName(string encodedName)
        {
            var service = await _dbContext.Services
                .Include(s => s.Offers)
                .Include(s => s.OpeningHours)
                .Include(s => s.Employees)
                    .ThenInclude(e => e.User)
                .Include(s => s.Opinions)
                .FirstOrDefaultAsync(s => s.EncodedName == encodedName);

   
             if (service != null)
            {
                if (service.Opinions.Any())
                {
                    service.AverageRating = Math.Round(service.Opinions.Average(o => o.Rating), 1);
                    service.OpinionsCount = service.Opinions.Count;
                }
                else
                {
                    service.AverageRating = 0.0;
                    service.OpinionsCount = 0;
                }
            }
            
            return service;
        }

        public async Task<Service> GetFirstServiceWithOpeningHours()
        {
            return await _dbContext.Services
                .Include(s => s.Offers)
                .Include(s => s.OpeningHours)
                .FirstOrDefaultAsync();
        }

        public async Task<Service?> GetServiceByEncodedName(string encodedName) =>
            await _dbContext.Services.FirstOrDefaultAsync(s => s.EncodedName == encodedName);
    

        public async Task<Service> GetServiceByIdAsync(int serviceId)
        {
            return await _dbContext.Services
                                  .Include(s => s.Offers)
                                  .Include(s => s.OpeningHours)
                                  .Include(s => s.Employees)
                                  .Include(s => s.Opinions) // Include opinions
                                  .Include(s => s.ServiceCategory) // Include ServiceCategory
                                  .FirstOrDefaultAsync(s => s.Id == serviceId);
        }

        public async Task<List<Offer>> SearchOffersAsync(string term)
        {
            term = term.ToLower();
            return await _dbContext.Offers
                .Where(o => o.Name.ToLower().Contains(term))
                .OrderBy(o => o.Name.ToLower().StartsWith(term) ? 0 : 1) 
                .ToListAsync();
        }

        public async Task<List<string>> SearchCitiesAsync(string term)
        {
            term = term.ToLower();
            return await _dbContext.Services
                .Where(s => s.ContactDetails.City.ToLower().Contains(term))
                .Select(s => s.ContactDetails.City)
                .Distinct()
                .OrderBy(c => c.StartsWith(term) ? 0 : 1)
                .ToListAsync();
        }

        public async Task<List<Service>> SearchServicesAsync(string searchTerm, string city)
        {
            var servicesQuery = _dbContext.Services.AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                string lowerSearchTerm = searchTerm.ToLower();
                servicesQuery = servicesQuery.Where(s => s.Offers.Any(o => o.Name.ToLower().Contains(lowerSearchTerm)));
            }

            if (!string.IsNullOrWhiteSpace(city))
            {
                string lowerCity = city.ToLower();
                servicesQuery = servicesQuery.Where(s => s.ContactDetails.City.ToLower().Contains(lowerCity));
            }

            var servicesWithDetails = await servicesQuery
                .Select(s => new
                {
                    Service = s,
                    OpinionsCount = s.Opinions.Count,
                    AverageRating = s.Opinions.Any() ? Math.Round(s.Opinions.Average(o => o.Rating), 1) : 0
                })
                .ToListAsync();

            var services = servicesWithDetails
                .Select(s =>
                {
                    s.Service.OpinionsCount = s.OpinionsCount;
                    s.Service.AverageRating = s.AverageRating;
                    return s.Service;
                })
                .ToList();

            return services;
        }

        public async Task<List<Service>> GetRecommendedServicesAsync()
        {
            var services = await _dbContext.Services
                .Include(s => s.ContactDetails)
                .Include(s => s.Opinions)
                .AsNoTracking()
                .Select(s => new
                {
                    s.Id,
                    s.Name,
                    s.ImageUrl,
                    ContactDetails = s.ContactDetails,
                    AverageRating = s.Opinions.Average(o => o.Rating),
                    OpinionsCount = s.Opinions.Count
                })
                .OrderByDescending(s => s.AverageRating * s.OpinionsCount) // Example scoring: average rating * number of opinions
                .Take(10)
                .ToListAsync();

            var result = services.Select(s =>
            {
                var service = new Service
                {
                    Id = s.Id,
                    Name = s.Name,
                    ImageUrl = s.ImageUrl,
                    ContactDetails = s.ContactDetails,
                    AverageRating = s.AverageRating,
                    OpinionsCount = s.OpinionsCount
                };
                service.EncodeName(); // Generate EncodedName
                return service;
            }).ToList();

            return result;
        }

        public async Task DeleteAsync(Service service)
        {
            if (service != null)
            {
              
                // Pobieramy opinie powiązane z pracownikami w serwisie
                var employeeIds = _dbContext.Employees
                    .Where(e => e.ServiceId == service.Id)
                    .Select(e => e.Id)
                    .ToList();

                var employeeOpinions = _dbContext.Opinions
                    .Where(o => employeeIds.Contains(o.EmployeeId.Value))
                    .ToList();
                _dbContext.Opinions.RemoveRange(employeeOpinions);

                // Usuwamy pracowników powiązanych z serwisem
                var employees = _dbContext.Employees
                    .Where(e => e.ServiceId == service.Id)
                    .ToList();
                _dbContext.Employees.RemoveRange(employees);

                // Usuwamy rezerwacje powiązane z ofertami serwisu
                var bookings = _dbContext.Bookings
                    .Where(b => b.Offer.ServiceId == service.Id)
                    .ToList();
                _dbContext.Bookings.RemoveRange(bookings);

                // Usuwamy opinie powiązane z serwisem
                var serviceOpinions = _dbContext.Opinions
                    .Where(o => o.ServiceId == service.Id)
                    .ToList();
                _dbContext.Opinions.RemoveRange(serviceOpinions);

                // Usuwamy serwis
                _dbContext.Services.Remove(service);
                await _dbContext.SaveChangesAsync();
            }
        }



        public async Task<IEnumerable<Service>> GetAllAsync()
        {
            return await _dbContext.Services
                .Include(s => s.ServiceCategory)
                .ToListAsync();
        }

        public async Task<List<Service>> SearchServicesAsync(string searchTerm)
        {
            searchTerm = searchTerm.ToLower();
            return await _dbContext.Services
                .Where(s => s.Name.ToLower().Contains(searchTerm))
                .Include(s => s.ServiceCategory)
                .ToListAsync();
        }

        public async Task AddAsync(Service service)
        {
            _dbContext.Services.Add(service);
            await _dbContext.SaveChangesAsync();
        }

        public Task Commit()
        => _dbContext.SaveChangesAsync();

        public async Task<List<Employee>> GetEmployeesByServiceIdAsync(int serviceId)
        {
            return await _dbContext.Employees
                .Where(e => e.ServiceId == serviceId)
                .Include(e => e.User)
                .ToListAsync();
        }

        public async Task<List<Offer>> GetOffersByServiceIdAsync(int serviceId)
        {
            return await _dbContext.Offers
                .Where(o => o.ServiceId == serviceId)
                .ToListAsync();
        }

        public async Task<Service?> GetByNameAsync(string name)
        {
            return await _dbContext.Services
                .FirstOrDefaultAsync(s => s.Name == name);
        }
    }
}
