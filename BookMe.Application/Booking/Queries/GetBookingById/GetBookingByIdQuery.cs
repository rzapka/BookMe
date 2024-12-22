using BookMe.Application.Booking.Dto;
using MediatR;

namespace BookMe.Application.Booking.Queries.GetBookingById
{
    public class GetBookingByIdQuery : IRequest<Domain.Entities.Booking>
    {
        public int Id { get; set; }
    }
}
