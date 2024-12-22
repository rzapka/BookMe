using MediatR;

namespace BookMe.Application.Offer.Commands.DeleteOffer
{
    public class DeleteOfferCommand : IRequest<Unit>
    {
        public int Id { get; set; }
    }
}
