using BookMe.Domain.Entities;
using BookMe.Domain.Interfaces;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace BookMe.Application.Service.Queries.SearchServices
{
    public class SearchServicesByOfferAndCityQueryHandler : IRequestHandler<SearchServicesByOfferAndCityQuery, List<Domain.Entities.Service>>
    {
        private readonly IServiceRepository _serviceRepository;

        public SearchServicesByOfferAndCityQueryHandler(IServiceRepository serviceRepository)
        {
            _serviceRepository = serviceRepository;
        }

        public async Task<List<Domain.Entities.Service>> Handle(SearchServicesByOfferAndCityQuery request, CancellationToken cancellationToken)
        {
            return await _serviceRepository.SearchServicesAsync(request.SearchTerm, request.City);
        }
    }
}
