using MediatR;

namespace BookMe.Application.Notification.Commands.MarkNotificationAsRead
{
    public class MarkNotificationAsReadCommand : IRequest<Unit>
    {
        public int NotificationId { get; set; }
    }
}
