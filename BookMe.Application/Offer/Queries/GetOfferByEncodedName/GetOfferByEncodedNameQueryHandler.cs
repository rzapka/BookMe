using BookMe.Application.Offer.Queries.GetOfferByEncodedName;
using BookMe.Domain.Entities;
using BookMe.Domain.Interfaces;
using MediatR;

public class GetOfferByEncodedNameQueryHandler : IRequestHandler<GetOfferByEncodedNameQuery, Offer>
{
    private readonly IOfferRepository _offerRepository;

    public GetOfferByEncodedNameQueryHandler(IOfferRepository offerRepository)
    {
        _offerRepository = offerRepository;
    }

    public async Task<Offer> Handle(GetOfferByEncodedNameQuery request, CancellationToken cancellationToken)
    {
        return await _offerRepository.GetByEncodedNames(request.ServiceEncodedName, request.OfferEncodedName);
    }
}
