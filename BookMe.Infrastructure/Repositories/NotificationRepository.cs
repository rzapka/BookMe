using BookMe.Domain.Entities;
using BookMe.Domain.Interfaces;
using BookMe.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BookMe.Infrastructure.Repositories
{
    public class NotificationRepository : INotificationRepository
    {
        private readonly BookMeDbContext _dbContext;

        public NotificationRepository(BookMeDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task CreateAsync(Notification notification)
        {
            await _dbContext.Notifications.AddAsync(notification);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<Notification>> GetNotificationsForEmployeeAsync(int employeeId)
        {
            return await _dbContext.Notifications
                .Where(n => n.EmployeeId == employeeId)
                .OrderByDescending(n => n.CreatedAt)
                .ToListAsync();
        }

        public async Task<Notification> GetByIdAsync(int id)
        {
            return await _dbContext.Notifications.FindAsync(id);
        }

        public async Task UpdateAsync(Notification notification)
        {
            _dbContext.Notifications.Update(notification);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<int> GetUnreadNotificationsCountAsync(int employeeId)
        {
            return await _dbContext.Notifications
                .Where(n => n.EmployeeId == employeeId && !n.IsRead)
                .CountAsync();
        }

        public async Task<int> GetUnreadNotificationCountForEmployeeAsync(int employeeId)
        {
            return await _dbContext.Notifications.CountAsync(n => n.EmployeeId == employeeId && !n.IsRead);
        }
    }
}
