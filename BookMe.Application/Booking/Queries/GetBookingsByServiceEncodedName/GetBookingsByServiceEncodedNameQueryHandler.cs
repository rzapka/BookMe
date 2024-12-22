using AutoMapper;
using BookMe.Application.Booking.Dto;
using MediatR;


namespace BookMe.Application.Booking.Queries.GetBookingsByServiceEncodedName
{
    public class GetBookingsByServiceEncodedNameQueryHandler : IRequestHandler<GetBookingsByServiceEncodedNameQuery, IEnumerable<BookingDto>>
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly IMapper _mapper;

        public GetBookingsByServiceEncodedNameQueryHandler(IBookingRepository bookingRepository, IMapper mapper)
        {
            _bookingRepository = bookingRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<BookingDto>> Handle(GetBookingsByServiceEncodedNameQuery request, CancellationToken cancellationToken)
        {
            var bookings = await _bookingRepository.GetBookingsByServiceEncodedName(request.EncodedName, request.SearchTerm);

            var bookingDtos = _mapper.Map<IEnumerable<BookingDto>>(bookings);

            return bookingDtos;
        }
    }
}
