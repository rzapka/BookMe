using BookMe.Domain.Entities;
using BookMe.Domain.Interfaces;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace BookMe.Application.Notification.Commands.CreateNotification
{
    public class CreateNotificationCommandHandler : IRequestHandler<CreateNotificationCommand, Unit>
    {
        private readonly INotificationRepository _notificationRepository;

        public CreateNotificationCommandHandler(INotificationRepository notificationRepository)
        {
            _notificationRepository = notificationRepository;
        }

        public async Task<Unit> Handle(CreateNotificationCommand request, CancellationToken cancellationToken)
        {
            var notification = new Domain.Entities.Notification
            {
                EmployeeId = request.EmployeeId,
                Message = request.Message
            };

            await _notificationRepository.CreateAsync(notification);

            return Unit.Value;
        }
    }
}
