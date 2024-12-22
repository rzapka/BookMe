using BookMe.Domain.Interfaces;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace BookMe.Application.ServiceImage.Commands.UpdateServiceImage
{
    public class UpdateServiceImageCommandHandler : IRequestHandler<UpdateServiceImageCommand>
    {
        private readonly IServiceImageRepository _serviceImageRepository;

        public UpdateServiceImageCommandHandler(IServiceImageRepository serviceImageRepository)
        {
            _serviceImageRepository = serviceImageRepository;
        }

        public async Task<Unit> Handle(UpdateServiceImageCommand request, CancellationToken cancellationToken)
        {
            var serviceImage = await _serviceImageRepository.GetServiceImageByIdAsync(request.Id);
            if (serviceImage == null)
            {
                throw new KeyNotFoundException("Nie znaleziono zdjęcia o podanym Id.");
            }

            serviceImage.Url = request.Url;
            await _serviceImageRepository.UpdateServiceImageAsync(serviceImage);

            return Unit.Value;
        }
    }
}
