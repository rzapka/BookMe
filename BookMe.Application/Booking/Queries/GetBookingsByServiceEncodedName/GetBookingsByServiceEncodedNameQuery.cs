using BookMe.Application.Booking.Dto;
using MediatR;
using System.Collections.Generic;

namespace BookMe.Application.Booking.Queries.GetBookingsByServiceEncodedName
{
    public class GetBookingsByServiceEncodedNameQuery : IRequest<IEnumerable<BookingDto>>
    {
        public string EncodedName { get; set; }
        public string SearchTerm { get; set; } 
    }
}
