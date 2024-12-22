using BookMe.Application.ServiceImage.Queries.GetServiceImagesByEncodedName;
using BookMe.Domain.Entities;
using BookMe.Domain.Interfaces;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace BookMe.Application.ServiceImage.Queries.GetServiceImagesByEncodedName
{
    public class GetServiceImagesByEncodedNameQueryHandler : IRequestHandler<GetServiceImagesByEncodedNameQuery, List<Domain.Entities.ServiceImage>>
    {
        private readonly IServiceImageRepository _serviceImageRepository;

        public GetServiceImagesByEncodedNameQueryHandler(IServiceImageRepository serviceImageRepository)
        {
            _serviceImageRepository = serviceImageRepository;
        }

        public async Task<List<Domain.Entities.ServiceImage>> Handle(GetServiceImagesByEncodedNameQuery request, CancellationToken cancellationToken)
        {
            return await _serviceImageRepository.GetServiceImagesByEncodedNameAsync(request.EncodedName);
        }
    }
}
