using Azure.Core;
using BookMe.Domain.Entities;
using BookMe.Domain.Interfaces;
using BookMe.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace BookMe.Infrastructure.Repositories
{
    public class OpeningHoursRepository : IOpeningHoursRepository
    {
        private readonly BookMeDbContext _dbContext;

        public OpeningHoursRepository(BookMeDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<OpeningHour>> GetByServiceIdAsync(int serviceId)
        {
            return await _dbContext.OpeningHours
                .Where(oh => oh.ServiceId == serviceId)
                .ToListAsync();
        }

        public async Task<OpeningHour> GetByIdAsync(int id)
        {
            return await _dbContext.OpeningHours
                .Include(o => o.Service)
                .FirstOrDefaultAsync(o => o.Id == id); 
        }

        public async Task AddAsync(OpeningHour openingHour)
        {
            _dbContext.OpeningHours.Add(openingHour);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(OpeningHour openingHour)
        {
            _dbContext.OpeningHours.Update(openingHour);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var openingHour = await _dbContext.OpeningHours.FindAsync(id);
            if (openingHour != null)
            {
                _dbContext.OpeningHours.Remove(openingHour);
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<OpeningHour>> GetOpeningHoursByServiceEncodedNameAsync(string encodedName)
        {
            return await _dbContext.OpeningHours
                .Include(o => o.Service)
                .Where(o => o.Service.EncodedName == encodedName)
                .ToListAsync();
        }

        public async Task<List<string>> GetTakenDaysOfWeekByServiceId(int serviceId)
        {
            return await _dbContext.OpeningHours
               .Where(o => o.ServiceId == serviceId)
                .Select(o => o.DayOfWeek)
               .ToListAsync();
        }
    }
}
