using BookMe.Domain.Entities;
using MediatR;

namespace BookMe.Application.Offer.Queries.GetOfferByEncodedName
{
    public class GetOfferByEncodedNameQuery : IRequest<Domain.Entities.Offer>
    {
        public string ServiceEncodedName { get; set; } = string.Empty;
        public string OfferEncodedName { get; set; } = string.Empty;
    }
}
