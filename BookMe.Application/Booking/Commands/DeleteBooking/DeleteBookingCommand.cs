using MediatR;

namespace BookMe.Application.Booking.Commands.DeleteBooking
{
    public class DeleteBookingCommand : IRequest<Unit>
    {
        public int Id { get; set; }
    }
}
