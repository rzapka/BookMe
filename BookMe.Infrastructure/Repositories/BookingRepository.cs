using BookMe.Domain.Entities;
using BookMe.Domain.Interfaces;
using BookMe.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookMe.Infrastructure.Repositories
{
    public class BookingRepository : IBookingRepository
    {
        private readonly BookMeDbContext _dbContext;

        public BookingRepository(BookMeDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task Create(Booking booking)
        {
            _dbContext.Add(booking);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<Booking>> GetBookingsByUserId(string userId)
        {
            return await _dbContext.Bookings
                .Include(b => b.Offer)
                    .ThenInclude(o => o.Service)
                .Include(b => b.Employee)
                    .ThenInclude(e => e.User)
                .Include(b => b.Opinion)
                .Where(b => b.UserId == userId)
                .OrderByDescending(b => b.StartTime)
                .ToListAsync();
        }

        public async Task<IEnumerable<Booking>> GetBookingsByEmployeeId(int employeeId)
        {
            return await _dbContext.Bookings
                .Include(b => b.Offer)
                .Include(b => b.Employee)
                .Where(b => b.EmployeeId == employeeId)
                .OrderByDescending(b => b.StartTime)
                .ToListAsync();
        }

        public async Task<Booking> GetBookingByIdAsync(int id)
        {
            return await _dbContext.Bookings
                .Include(e => e.User)
                .Include(b => b.Offer)
                    .ThenInclude(o => o.Service)
                .Include(b => b.Employee)
                    .ThenInclude(e => e.User)

                .FirstOrDefaultAsync(b => b.Id == id);
        }

        public async Task UpdateBookingAsync(Booking booking)
        {
            _dbContext.Bookings.Update(booking);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteBookingAsync(int id)
        {
    
            var booking = await _dbContext.Bookings
                .Include(b => b.Opinion) 
                .FirstOrDefaultAsync(b => b.Id == id);

            if (booking != null)
            {
                if (booking.Opinion != null)
                {
                    _dbContext.Opinions.Remove(booking.Opinion);
                }

                _dbContext.Bookings.Remove(booking);

                await _dbContext.SaveChangesAsync();
            }
        }


        public async Task<Opinion> GetOpinionByBookingIdAsync(int bookingId)
        {
            return await _dbContext.Opinions.FirstOrDefaultAsync(o => o.BookingId == bookingId);
        }



        public async Task<bool> HasOpinion(int bookingId)
        {
            var booking = await _dbContext.Bookings
                .Include(b => b.Opinion) 
                .FirstOrDefaultAsync(b => b.Id == bookingId);

            return booking?.Opinion != null; 
        }

        public async Task<List<Booking>> GetBookingsByEmployeeIdAsync(int employeeId)
        {
            return await _dbContext.Bookings
                .Include(b => b.Offer)
                .Include(b => b.User)
                .Include(b => b.Employee)
                .Where(b => b.EmployeeId == employeeId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Booking>> GetBookingsByServiceEncodedName(string encodedName, string searchTerm = null)
        {
            var query = _dbContext.Bookings
                .Include(b => b.Offer)
                    .ThenInclude(o => o.Service)
                .Include(b => b.Employee)
                    .ThenInclude(e => e.User)
                .Include(b => b.User)
                .Where(b => b.Offer.Service.EncodedName == encodedName && b.StartTime > DateTime.Now);

            if (!string.IsNullOrEmpty(searchTerm))
            {
                var normalizedSearchTerm = searchTerm.ToLower();

                query = query.Where(b =>
                    (b.User.FirstName + " " + b.User.LastName).ToLower().Contains(normalizedSearchTerm));
            }

            return await query
                .OrderBy(b => b.StartTime)
                .ToListAsync();
        }



    }
}
