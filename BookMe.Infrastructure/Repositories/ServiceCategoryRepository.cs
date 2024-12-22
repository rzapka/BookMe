using BookMe.Domain.Entities;
using BookMe.Domain.Interfaces;
using BookMe.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BookMe.Infrastructure.Repositories
{
    public class ServiceCategoryRepository : IServiceCategoryRepository
    {
        private readonly BookMeDbContext _dbContext;

        public ServiceCategoryRepository(BookMeDbContext bookMeDbContext)
        {
            _dbContext = bookMeDbContext;
        }

        public async Task<IEnumerable<ServiceCategory>> GetAll()
        {
            return await _dbContext.ServiceCategories.ToListAsync();
        }

        public async Task<ServiceCategory?> GetByIdAsync(int id)
        {
            return await _dbContext.ServiceCategories
                .Include(sc => sc.Services)
                    .ThenInclude(s => s.Employees)
                .Include(sc => sc.Services)
                    .ThenInclude(s => s.Opinions)
                .FirstOrDefaultAsync(sc => sc.Id == id);
        }

        public async Task<ServiceCategory> GetByEncodedName(string encodedName, string searchTerm = "")
        {
            searchTerm = searchTerm.ToLower();
            if (encodedName == "inne")
            {
                // Pobierz wszystkie serwisy, które nie mają przypisanego ServiceCategoryId
                var servicesWithoutCategory = await _dbContext.Services
                    .Where(s => s.ServiceCategoryId == null &&
                                (string.IsNullOrWhiteSpace(searchTerm)
                                || s.Name.ToLower().Contains(searchTerm)
                                || s.ContactDetails.Street.ToLower().Contains(searchTerm)
                                || s.ContactDetails.City.ToLower().Contains(searchTerm)))
                    .Select(s => new {
                        Service = s,
                        OpinionsCount = s.Opinions.Count,
                        AverageRating = s.Opinions.Any() ? Math.Round(s.Opinions.Average(o => o.Rating), 1) : 0
                    }).ToListAsync();

                // Stwórz fikcyjną kategorię "Inne"
                var serviceCategory = new ServiceCategory
                {
                    Name = "Inne",
                    Services = servicesWithoutCategory
                        .Select(s => {
                            var service = s.Service;
                            service.OpinionsCount = s.OpinionsCount;
                            service.AverageRating = s.AverageRating;
                            return service;
                        }).ToList()
                };
                
                serviceCategory.EncodeName();

                return serviceCategory;
            }
            
            IQueryable<ServiceCategory> query = _dbContext.ServiceCategories.AsQueryable();

            var serviceCategoryWithDetails = await query
                .Where(sc => sc.EncodedName == encodedName)
                .Select(sc => new {
                    ServiceCategory = sc,
                    Services = sc.Services
                                 .Where(s => string.IsNullOrWhiteSpace(searchTerm)
                                          || s.Name.ToLower().Contains(searchTerm)
                                          || s.ContactDetails.Street.ToLower().Contains(searchTerm)
                                          || s.ContactDetails.City.ToLower().Contains(searchTerm))
                                 .Select(s => new {
                                     Service = s,
                                     OpinionsCount = s.Opinions.Count,
                                     AverageRating = s.Opinions.Any() ? Math.Round(s.Opinions.Average(o => o.Rating), 1) : 0
                                 }).ToList()
                })
                .FirstOrDefaultAsync();
            
            if (serviceCategoryWithDetails != null)
            {
                var serviceCategory = serviceCategoryWithDetails.ServiceCategory;
                serviceCategory.Services = serviceCategoryWithDetails.Services
                    .Select(s => {
                        var service = s.Service;
                        service.OpinionsCount = s.OpinionsCount;
                        service.AverageRating = s.AverageRating;
                        return service;
                    }).ToList();

                return serviceCategory;
            }
            return new ServiceCategory
            {
                Name = encodedName,
                Services = new List<Service>()
            };
        }

        public async Task AddAsync(ServiceCategory category)
        {
            await _dbContext.ServiceCategories.AddAsync(category);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(ServiceCategory category)
        {
            _dbContext.ServiceCategories.Update(category);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(ServiceCategory category)
        {
            _dbContext.ServiceCategories.Remove(category);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<ServiceCategory>> SearchAsync(string searchTerm)
        {
            searchTerm = searchTerm.ToLower();

            return await _dbContext.ServiceCategories
                .Where(c => c.Name.ToLower().Contains(searchTerm))
                .ToListAsync();
        }
    }
}
