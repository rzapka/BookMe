using BookMe.Domain.Interfaces;
using MediatR;

namespace BookMe.Application.ServiceImage.Queries.GetServiceImagesByServiceId
{
    public class GetServiceImagesByServiceIdQueryHandler : IRequestHandler<GetServiceImagesByServiceIdQuery, List<Domain.Entities.ServiceImage>>
    {
        private readonly IServiceImageRepository _serviceImageRepository;

        public GetServiceImagesByServiceIdQueryHandler(IServiceImageRepository serviceImageRepository)
        {
            _serviceImageRepository = serviceImageRepository;
        }
        public Task<List<Domain.Entities.ServiceImage>> Handle(GetServiceImagesByServiceIdQuery request, CancellationToken cancellationToken)
        {
            return _serviceImageRepository.GetServiceImagesByServiceIdAsync(request.ServiceId);
        }
    }
}
