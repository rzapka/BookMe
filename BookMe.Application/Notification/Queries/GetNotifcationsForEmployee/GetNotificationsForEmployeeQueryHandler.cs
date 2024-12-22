using AutoMapper;
using BookMe.Application.Notification.Dto;
using BookMe.Domain.Interfaces;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace BookMe.Application.Notification.Queries.GetNotifcationsForEmployee
{
    public class GetNotificationsForEmployeeQueryHandler : IRequestHandler<GetNotificationsForEmployeeQuery, IEnumerable<NotificationDto>>
    {
        private readonly INotificationRepository _notificationRepository;
        private readonly IMapper _mapper;

        public GetNotificationsForEmployeeQueryHandler(INotificationRepository notificationRepository, IMapper mapper)
        {
            _notificationRepository = notificationRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<NotificationDto>> Handle(GetNotificationsForEmployeeQuery request, CancellationToken cancellationToken)
        {
            var notifications = await _notificationRepository.GetNotificationsForEmployeeAsync(request.EmployeeId);
            return _mapper.Map<IEnumerable<NotificationDto>>(notifications);
        }
    }
}
