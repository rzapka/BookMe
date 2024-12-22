using BookMe.Domain.Interfaces;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace BookMe.Application.Service.Queries.SearchCities
{
    public class SearchCitiesQueryHandler : IRequestHandler<SearchCitiesQuery, List<string>>
    {
        private readonly IServiceRepository _serviceRepository;

        public SearchCitiesQueryHandler(IServiceRepository serviceRepository)
        {
            _serviceRepository = serviceRepository;
        }

        public async Task<List<string>> Handle(SearchCitiesQuery request, CancellationToken cancellationToken)
        {
            return await _serviceRepository.SearchCitiesAsync(request.Term);
        }
    }
}
