using BookMe.Domain.Entities;
using BookMe.Domain.Interfaces;
using BookMe.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BookMe.Infrastructure.Repositories
{
    public class OfferRepository : IOfferRepository
    {
        private readonly BookMeDbContext _dbContext;

        public OfferRepository(BookMeDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Offer> GetByEncodedNames(string serviceEncodedName, string offerEncodedName)
        {
       
            var service = await _dbContext.Services
                .FirstOrDefaultAsync(s => s.EncodedName == serviceEncodedName);
   
            return await _dbContext.Offers
                .Include(o => o.Service) 
                .FirstOrDefaultAsync(o => o.EncodedName == offerEncodedName && o.ServiceId == service.Id);
        }

        public async Task<Offer> GetByIdAsync(int id)
        {
            return await _dbContext.Offers
                .Include(o => o.Service)
                .FirstOrDefaultAsync(o => o.Id == id);
        }

        public async Task<IEnumerable<Offer>> GetByServiceIdAsync(int serviceId)
        {
            return await _dbContext.Offers
                .Where(o => o.ServiceId == serviceId)
                .ToListAsync();
        }

        public async Task AddAsync(Offer offer)
        {
            _dbContext.Offers.Add(offer);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(Offer offer)
        {
            _dbContext.Offers.Update(offer);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var offer = await _dbContext.Offers.FindAsync(id);

            if (offer != null)
            {

                var bookings = await _dbContext.Bookings
                    .Where(b => b.OfferId == id)
                    .ToListAsync();

                _dbContext.Bookings.RemoveRange(bookings);

 
                _dbContext.Offers.Remove(offer);

 
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Offer>> GetByServiceEncodedNameAsync(string encodedName)
        {
            return await _dbContext.Offers
                .Include (o => o.Service)
                .Where(o => o.Service.EncodedName == encodedName)
                .ToListAsync();
        }

     
    }
}
