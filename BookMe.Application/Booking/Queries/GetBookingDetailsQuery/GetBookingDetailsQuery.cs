using BookMe.Application.Booking.Dto;
using MediatR;

namespace BookMe.Application.Booking.Queries.GetBookingDetails
{
    public class GetBookingDetailsQuery : IRequest<BookingDto>
    {
        public int BookingId { get; set; }
    }
}
