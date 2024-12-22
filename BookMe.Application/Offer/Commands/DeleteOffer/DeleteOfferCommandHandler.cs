using BookMe.Domain.Interfaces;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace BookMe.Application.Offer.Commands.DeleteOffer
{
    public class DeleteOfferCommandHandler : IRequestHandler<DeleteOfferCommand>
    {
        private readonly IOfferRepository _offerRepository;

        public DeleteOfferCommandHandler(IOfferRepository offerRepository)
        {
            _offerRepository = offerRepository;
        }

        public async Task<Unit> Handle(DeleteOfferCommand request, CancellationToken cancellationToken)
        {
            var offer = await _offerRepository.GetByIdAsync(request.Id);
            if (offer == null)
            {
                throw new KeyNotFoundException("Offer not found.");
            }

            await _offerRepository.DeleteAsync(request.Id);
            return Unit.Value;
        }
    }
}
