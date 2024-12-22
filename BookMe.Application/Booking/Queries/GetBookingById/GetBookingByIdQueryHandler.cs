using AutoMapper;
using BookMe.Application.Booking.Queries.GetBookingById;
using BookMe.Domain.Entities;
using MediatR;

public class GetBookingByIdQueryHandler : IRequestHandler<GetBookingByIdQuery, Booking>
{
    private readonly IBookingRepository _bookingRepository;

    public GetBookingByIdQueryHandler(IBookingRepository bookingRepository, IMapper mapper)
    {
        _bookingRepository = bookingRepository;
    }

    public async Task<Booking> Handle(GetBookingByIdQuery request, CancellationToken cancellationToken)
    {
        return await _bookingRepository.GetBookingByIdAsync(request.Id);
    }
}
