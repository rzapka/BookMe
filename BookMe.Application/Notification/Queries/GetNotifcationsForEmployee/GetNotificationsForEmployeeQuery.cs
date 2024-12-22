using BookMe.Application.Notification.Dto;
using MediatR;
using System.Collections.Generic;

namespace BookMe.Application.Notification.Queries.GetNotifcationsForEmployee
{
    public class GetNotificationsForEmployeeQuery : IRequest<IEnumerable<NotificationDto>>
    {
        public int EmployeeId { get; set; }
    }
}
