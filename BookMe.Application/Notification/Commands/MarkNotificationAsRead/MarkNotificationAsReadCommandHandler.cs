using BookMe.Domain.Entities;
using BookMe.Domain.Interfaces;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace BookMe.Application.Notification.Commands.MarkNotificationAsRead
{
    public class MarkNotificationAsReadCommandHandler : IRequestHandler<MarkNotificationAsReadCommand>
    {
        private readonly INotificationRepository _notificationRepository;

        public MarkNotificationAsReadCommandHandler(INotificationRepository notificationRepository)
        {
            _notificationRepository = notificationRepository;
        }

        public async Task<Unit> Handle(MarkNotificationAsReadCommand request, CancellationToken cancellationToken)
        {
            var notification = await _notificationRepository.GetByIdAsync(request.NotificationId);
            if (notification == null)
            {
                throw new KeyNotFoundException("Notification not found.");
            }

            notification.IsRead = true;
            await _notificationRepository.UpdateAsync(notification);

            return Unit.Value;
        }
    }
}
