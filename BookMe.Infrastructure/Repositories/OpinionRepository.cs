using BookMe.Domain.Entities;
using BookMe.Domain.Interfaces;
using BookMe.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookMe.Infrastructure.Repositories
{
    public class OpinionRepository : IOpinionRepository
    {
        private readonly BookMeDbContext _dbContext;

        public OpinionRepository(BookMeDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task CreateOpinionAsync(Opinion opinion)
        {
            _dbContext.Opinions.Add(opinion);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateOpinionAsync(Opinion opinion)
        {
            _dbContext.Opinions.Update(opinion);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<Opinion> GetOpinionByIdAsync(int id)
        {
            return await _dbContext.Opinions
                .Include(o => o.Service)
                .Include(o => o.Offer)
                .Include(o => o.Booking)
                .Include(o => o.Employee).ThenInclude(e => e.User)
                .FirstOrDefaultAsync(o => o.Id == id);
        }

        public async Task<IEnumerable<Opinion>> GetOpinionsByServiceIdAsync(int serviceId)
        {
            return await _dbContext.Opinions
                .Include(o => o.User)
                .Include(o => o.Service)
                .Include(o => o.Offer)
                .Include(o => o.Employee).ThenInclude(e => e.User)
                .Where(o => o.ServiceId == serviceId)
                .ToListAsync();
        }

        public async Task DeleteOpinionAsync(int id)
        {
            var opinion = await _dbContext.Opinions.FindAsync(id);
            if (opinion != null)
            {
                _dbContext.Opinions.Remove(opinion);
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task DeleteAsync(int opinionId)
        {
            var opinion = await _dbContext.Opinions.FindAsync(opinionId);
            if (opinion != null)
            {
                _dbContext.Opinions.Remove(opinion);
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Opinion>> GetOpinionsByEmployeeIdAsync(int employeeId)
        {
            return await _dbContext.Opinions
                .Where(o => o.EmployeeId == employeeId)
                .Include(o => o.User)
                .Include(o => o.Service)
                .Include(o => o.Offer)
                .Include(o => o.Employee).ThenInclude(e => e.User)
                .ToListAsync();
        }

        public async Task<Service?> GetServiceByOpinionId(int opinionId)
        {
            var opinion = await _dbContext.Opinions
                .Include(o => o.Service) 
                .FirstOrDefaultAsync(o => o.Id == opinionId);

            return opinion?.Service; 
        }

        public Task Commit()
            => _dbContext.SaveChangesAsync();

        public async Task<IEnumerable<Opinion>> GetOpinionsByServiceEncodedNameAsync(string encodedName)
        {
            return await _dbContext.Opinions
                .Include(o => o.User)
                .Include(o => o.Employee).ThenInclude(e => e.User)
                .Include(o => o.Offer)
                .Include(o => o.Service)
                .Where(o => o.Service.EncodedName == encodedName)
                .OrderByDescending(o => o.CreatedAt)
                .ToListAsync();
        }
    }
}
