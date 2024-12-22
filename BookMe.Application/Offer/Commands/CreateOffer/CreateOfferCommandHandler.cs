using BookMe.Domain.Entities;
using BookMe.Domain.Interfaces;
using FluentValidation;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace BookMe.Application.Offer.Commands.CreateOffer
{
    public class CreateOfferCommandHandler : IRequestHandler<CreateOfferCommand>
    {
        private readonly IOfferRepository _offerRepository;

        public CreateOfferCommandHandler(IOfferRepository offerRepository)
        {
            _offerRepository = offerRepository;
        }

        public async Task<Unit> Handle(CreateOfferCommand request, CancellationToken cancellationToken)
        {
            var offer = new Domain.Entities.Offer
            {
                Name = request.Name,
                Duration = request.Duration,
                Price = request.Price,
                ServiceId = request.ServiceId,
            };
            offer.EncodeName();
            await _offerRepository.AddAsync(offer);
            return Unit.Value;
        }
    }
}
