using BookMe.Domain.Entities;
using MediatR;
using System.Collections.Generic;

namespace BookMe.Application.Booking.Queries.ListBookings
{
    public class ListBookingsQuery : IRequest<IEnumerable<Domain.Entities.Booking>>
    {
        public string UserId { get; set; } = string.Empty;
        public bool IsEmployee { get; set; }
    }
}
