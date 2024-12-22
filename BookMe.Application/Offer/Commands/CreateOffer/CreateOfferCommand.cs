using MediatR;

namespace BookMe.Application.Offer.Commands.CreateOffer
{
    public class CreateOfferCommand : IRequest<Unit>
    {
        public string Name { get; set; } = string.Empty;
        public int Duration { get; set; }
        public decimal Price { get; set; }
        public int ServiceId { get; set; }

        public string EncodedName { get; private set; } = string.Empty;

        public string ServiceEncodedName { get; set; } = string.Empty;
    }
}
