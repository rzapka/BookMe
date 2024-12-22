using AutoMapper;
using BookMe.Application.Offer.Commands.UpdateOffer;
using BookMe.Application.Offer.Dto;
using BookMe.Domain.Interfaces;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace BookMe.Application.Offer.Queries.GetOfferById
{
    public class GetOfferByIdQueryHandler : IRequestHandler<GetOfferByIdQuery, Domain.Entities.Offer>
    {
        private readonly IOfferRepository _offerRepository;

        public GetOfferByIdQueryHandler(IOfferRepository offerRepository)
        {
            _offerRepository = offerRepository;
        }

        public async Task<Domain.Entities.Offer> Handle(GetOfferByIdQuery request, CancellationToken cancellationToken)
        {
            return await _offerRepository.GetByIdAsync(request.Id);
        }
    }
}
