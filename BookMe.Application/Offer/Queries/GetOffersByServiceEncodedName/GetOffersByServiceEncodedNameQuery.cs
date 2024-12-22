using BookMe.Application.Offer.Dto;
using MediatR;
using System.Collections.Generic;

namespace BookMe.Application.Offer.Queries.GetOffersByServiceEncodedName
{
    public class GetOffersByServiceEncodedNameQuery : IRequest<IEnumerable<OfferDto>>
    {
        public string ServiceEncodedName { get; set; } = string.Empty;
    }
}
