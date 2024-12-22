using AutoMapper;
using BookMe.Application.Offer.Dto;
using BookMe.Domain.Interfaces;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace BookMe.Application.Offer.Queries.GetOffersByServiceId
{
    public class GetOffersByServiceIdQueryHandler : IRequestHandler<GetOffersByServiceIdQuery, IEnumerable<OfferDto>>
    {
        private readonly IOfferRepository _offerRepository;
        private readonly IMapper _mapper;

        public GetOffersByServiceIdQueryHandler(IOfferRepository offerRepository, IMapper mapper)
        {
            _offerRepository = offerRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<OfferDto>> Handle(GetOffersByServiceIdQuery request, CancellationToken cancellationToken)
        {
            var offers = await _offerRepository.GetByServiceIdAsync(request.ServiceId);
            return _mapper.Map<IEnumerable<OfferDto>>(offers);
        }
    }
}
