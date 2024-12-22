using AutoMapper;
using BookMe.Application.Offer.Dto;
using BookMe.Domain.Interfaces;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace BookMe.Application.Offer.Queries.GetOffersByServiceEncodedName
{
    public class GetOffersByServiceEncodedNameQueryHandler : IRequestHandler<GetOffersByServiceEncodedNameQuery, IEnumerable<OfferDto>>
    {
        private readonly IOfferRepository _offerRepository;
        private readonly IMapper _mapper;

        public GetOffersByServiceEncodedNameQueryHandler(IOfferRepository offerRepository, IMapper mapper)
        {
            _offerRepository = offerRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<OfferDto>> Handle(GetOffersByServiceEncodedNameQuery request, CancellationToken cancellationToken)
        {
            var offers = await _offerRepository.GetByServiceEncodedNameAsync(request.ServiceEncodedName);
            return _mapper.Map<IEnumerable<OfferDto>>(offers);
        }
    }
}
