using MediatR;

namespace BookMe.Application.Offer.Commands.UpdateOffer
{
    public class UpdateOfferCommand : IRequest<Unit>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Duration { get; set; }
        public decimal Price { get; set; }
        public string EncodedName { get; private set; } = string.Empty;

        public string ServiceEncodedName { get;  set; } = string.Empty;
    }
}
