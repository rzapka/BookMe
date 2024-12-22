using BookMe.Application.ApplicationUser;
using BookMe.Domain.Entities;
using BookMe.Domain.Interfaces;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace BookMe.Application.Booking.Queries.ListBookings
{
    public class ListBookingsQueryHandler : IRequestHandler<ListBookingsQuery, IEnumerable<Domain.Entities.Booking>>
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly IUserContext _userContext;

        public ListBookingsQueryHandler(IBookingRepository bookingRepository, IUserContext userContext)
        {
            _bookingRepository = bookingRepository;
            _userContext = userContext;
        }

        public async Task<IEnumerable<Domain.Entities.Booking>> Handle(ListBookingsQuery request, CancellationToken cancellationToken)
        {
            if (request.IsEmployee)
            {
                var employee = await _userContext.GetEmployeeByUserIdAsync(request.UserId);
                return await _bookingRepository.GetBookingsByEmployeeIdAsync(employee.Id);
            }
            else
            {
                return await _bookingRepository.GetBookingsByUserId(request.UserId);
            }
        }
    }
}
