using AutoMapper;
using BookMe.Application.Booking.Queries.GetBookingCreationData;
using BookMe.Application.Offer.Dto;
using BookMe.Domain.Interfaces;
using MediatR;

namespace BookMe.Application.Booking.Queries.GetBookingCreationData
{
    public class GetBookingCreationDataQueryHandler : IRequestHandler<GetBookingCreationDataQuery, BookingCreationDataResult>
    {
        private readonly IOfferRepository _offerRepository;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IMapper _mapper;

        public GetBookingCreationDataQueryHandler(IOfferRepository offerRepository,
            IEmployeeRepository employeeRepository,IMapper mapper)
        {
            _offerRepository = offerRepository;
            _mapper = mapper;
            _employeeRepository = employeeRepository;
        }

        public async Task<BookingCreationDataResult> Handle(GetBookingCreationDataQuery request, CancellationToken cancellationToken)
        {
            var employees = await _employeeRepository.GetEmployeesByServiceEncodedNameAsDictionary(request.ServiceEncodedName);

            var offer = await _offerRepository.GetByEncodedNames(request.ServiceEncodedName, request.OfferEncodedName);

            var offerDto = _mapper.Map<OfferDto>(offer);

            return new BookingCreationDataResult
            {
                Employees = employees,
                Offer = offerDto
            };
        }
    }
}
