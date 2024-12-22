using MediatR;

namespace BookMe.Application.Notification.Commands.CreateNotification
{
    public class CreateNotificationCommand : IRequest<Unit>
    {
        public int EmployeeId { get; set; }
        public string Message { get; set; } = string.Empty;
    }
}
