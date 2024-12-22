using BookMe.Domain.Entities;
using BookMe.Domain.Interfaces;
using BookMe.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookMe.Infrastructure.Repositories
{
    public class ApplicationUserRepository : IApplicationUserRepository
    {
        private readonly BookMeDbContext _context;

        public ApplicationUserRepository(BookMeDbContext context)
        {
            _context = context;
        }

        public async Task<ApplicationUser?> GetByIdAsync(string id)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<IEnumerable<ApplicationUser>> GetAllAsync()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<IEnumerable<ApplicationUser>> SearchAsync(string searchTerm)
        {
            searchTerm = searchTerm.ToLower();  
            return await _context.Users
                .Where(u => u.FirstName.ToLower().Contains(searchTerm) || u.LastName.ToLower().Contains(searchTerm))
                .ToListAsync();
        }

        public async Task<IEnumerable<ApplicationUser>> GetUsersWithoutEmployeesAsync()
        {
            var employeeUserIds = await _context.Employees.Select(e => e.UserId).ToListAsync();
            return await _context.Users
                .Where(u => !employeeUserIds.Contains(u.Id))
                .ToListAsync();
        }

        public async Task UpdateAsync(ApplicationUser user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(string id)
        {
            var user = await _context.Users
                .Include(u => u.Opinions)
                .Include(u => u.Bookings)
                .FirstOrDefaultAsync(u => u.Id == id);

            if (user != null)
            {
                // Usuń powiązane Opinions
                if (user.Opinions != null && user.Opinions.Any())
                {
                    _context.Opinions.RemoveRange(user.Opinions);
                }
                
                if (user.Bookings != null && user.Bookings.Any())
                {
                    _context.Bookings.RemoveRange(user.Bookings);
                }
                
                var employee = await _context.Employees
                    .FirstOrDefaultAsync(e => e.UserId == id);
                if (employee != null)
                {
                    _context.Employees.Remove(employee);
                }
                
                _context.Users.Remove(user);
                
                await _context.SaveChangesAsync();
            }
        }
    }
}
