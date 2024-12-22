using BookMe.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookMe.Domain.Interfaces
{
    public interface INotificationRepository
    {
        public Task CreateAsync(Notification notification);
            
        public Task<Notification> GetByIdAsync(int id);

        public Task UpdateAsync(Notification notification);

        public Task<IEnumerable<Notification>> GetNotificationsForEmployeeAsync(int employeeId);

        Task<int> GetUnreadNotificationsCountAsync(int employeeId);

        Task<int> GetUnreadNotificationCountForEmployeeAsync(int employeeId);

    }
}
