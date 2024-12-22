using BookMe.Application.Offer.Commands.UpdateOffer;
using BookMe.Application.Offer.Dto;
using MediatR;

namespace BookMe.Application.Offer.Queries.GetOfferById
{
    public class GetOfferByIdQuery : IRequest<Domain.Entities.Offer>
    {
        public int Id { get; set; }
    }
}
