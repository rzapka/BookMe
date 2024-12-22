using AutoMapper;
using BookMe.Application.Booking.Dto;
using BookMe.Domain.Interfaces;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace BookMe.Application.Booking.Queries.GetBookingDetails
{
    public class GetBookingDetailsQueryHandler : IRequestHandler<GetBookingDetailsQuery, BookingDto>
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly IOfferRepository _offerRepository;
        private readonly IMapper _mapper;

        public GetBookingDetailsQueryHandler(IBookingRepository bookingRepository, IOfferRepository offerRepository, IMapper mapper)
        {
            _bookingRepository = bookingRepository;
            _offerRepository = offerRepository;
            _mapper = mapper;
        }

        public async Task<BookingDto> Handle(GetBookingDetailsQuery request, CancellationToken cancellationToken)
        {
            var booking = await _bookingRepository.GetBookingByIdAsync(request.BookingId);

            if (booking != null)
            {
                booking.Opinion = await _bookingRepository.GetOpinionByBookingIdAsync(booking.Id);
                booking.Offer = await _offerRepository.GetByIdAsync(booking.OfferId);
            }

            var bookingDto = _mapper.Map<BookingDto>(booking);
            return bookingDto;
        }
    }
}
