using BookMe.Application.Offer.Dto;
using MediatR;
using System.Collections.Generic;

namespace BookMe.Application.Offer.Queries.GetOffersByServiceId
{
    public class GetOffersByServiceIdQuery : IRequest<IEnumerable<OfferDto>>
    {
        public int ServiceId { get; set; }
    }
}
