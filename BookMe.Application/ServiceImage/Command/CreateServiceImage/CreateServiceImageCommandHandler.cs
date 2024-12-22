using BookMe.Domain.Entities;
using BookMe.Domain.Interfaces;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace BookMe.Application.ServiceImage.Commands.CreateServiceImage
{
    public class CreateServiceImageCommandHandler : IRequestHandler<CreateServiceImageCommand>
    {
        private readonly IServiceImageRepository _serviceImageRepository;

        public CreateServiceImageCommandHandler(IServiceImageRepository serviceImageRepository)
        {
            _serviceImageRepository = serviceImageRepository;
        }

        public async Task<Unit> Handle(CreateServiceImageCommand request, CancellationToken cancellationToken)
        {
            var serviceImage = new Domain.Entities.ServiceImage
            {
                Url = request.Url,
                ServiceId = request.ServiceId
            };

            await _serviceImageRepository.AddServiceImageAsync(serviceImage);

            return Unit.Value;
        }
    }
}
