using BookMe.Domain.Interfaces;
using FluentValidation;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace BookMe.Application.Offer.Commands.UpdateOffer
{
    public class UpdateOfferCommandHandler : IRequestHandler<UpdateOfferCommand>
    {
        private readonly IOfferRepository _offerRepository;

        public UpdateOfferCommandHandler(IOfferRepository offerRepository)
        {
            _offerRepository = offerRepository;
        }

        public async Task<Unit> Handle(UpdateOfferCommand request, CancellationToken cancellationToken)
        {

            var offer = await _offerRepository.GetByIdAsync(request.Id);
            if (offer == null)
            {
                throw new KeyNotFoundException("Oferta nie została znaleziona.");
            }

            offer.Name = request.Name;
            offer.Duration = request.Duration;
            offer.Price = request.Price;
            offer.EncodeName();

            await _offerRepository.UpdateAsync(offer);
            return Unit.Value;
        }
    }
}
