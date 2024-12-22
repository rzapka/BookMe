using BookMe.Domain.Entities;
using BookMe.Domain.Interfaces;
using BookMe.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace BookMe.Infrastructure.Repositories
{
    public class ServiceImageRepository : IServiceImageRepository
    {
        private readonly BookMeDbContext _context;

        public ServiceImageRepository(BookMeDbContext context)
        {
            _context = context;
        }

        public async Task<List<ServiceImage>> GetServiceImagesByServiceIdAsync(int serviceId)
        {
            return await _context.ServiceImages
                .Where(image => image.ServiceId == serviceId)
                .ToListAsync();
        }

        public async Task<List<ServiceImage>> GetServiceImagesByEncodedNameAsync(string encodedName)
        {
            return await _context.ServiceImages
                .Include(si => si.Service)
                .Where(si => si.Service.EncodedName == encodedName)
                .ToListAsync();
        }

        public async Task<ServiceImage> GetServiceImageByIdAsync(int id)
        {
            return await _context.ServiceImages
                .Include(si => si.Service)
                .FirstOrDefaultAsync(si => si.Id == id);
        }

        public async Task AddServiceImageAsync(ServiceImage serviceImage)
        {
            _context.ServiceImages.Add(serviceImage);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateServiceImageAsync(ServiceImage serviceImage)
        {
            _context.ServiceImages.Update(serviceImage);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteServiceImageAsync(int id)
        {
            var serviceImage = await GetServiceImageByIdAsync(id);
            if (serviceImage != null)
            {
                _context.ServiceImages.Remove(serviceImage);
                await _context.SaveChangesAsync();
            }
            else
            {
                throw new KeyNotFoundException($"Nie znaleziono zdjęcia o Id: {id}");
            }
        }
    }
}
