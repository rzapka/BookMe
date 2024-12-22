using BookMe.Domain.Entities;
using BookMe.Domain.Interfaces;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace BookMe.Application.ServiceImage.Queries.GetServiceImageById
{
    public class GetServiceImageByIdQueryHandler : IRequestHandler<GetServiceImageByIdQuery, Domain.Entities.ServiceImage>
    {
        private readonly IServiceImageRepository _serviceImageRepository;

        public GetServiceImageByIdQueryHandler(IServiceImageRepository serviceImageRepository)
        {
            _serviceImageRepository = serviceImageRepository;
        }

        public async Task<Domain.Entities.ServiceImage> Handle(GetServiceImageByIdQuery request, CancellationToken cancellationToken)
        {
            var serviceImage = await _serviceImageRepository.GetServiceImageByIdAsync(request.Id);
            if (serviceImage == null)
            {
                throw new KeyNotFoundException($"Nie znaleziono zdjęcia o Id: {request.Id}");
            }

            return serviceImage;
        }
    }
}
