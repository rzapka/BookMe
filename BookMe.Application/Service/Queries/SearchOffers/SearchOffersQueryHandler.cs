using BookMe.Domain.Interfaces;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BookMe.Application.Service.Queries.SearchOffers
{
    public class SearchOffersQueryHandler : IRequestHandler<SearchOffersQuery, List<string>>
    {
        private readonly IServiceRepository _serviceRepository;

        public SearchOffersQueryHandler(IServiceRepository serviceRepository)
        {
            _serviceRepository = serviceRepository;
        }

        public async Task<List<string>> Handle(SearchOffersQuery request, CancellationToken cancellationToken)
        {
            var offers = await _serviceRepository.SearchOffersAsync(request.Term);
            return offers.Select(o => o.Name).Distinct().ToList();
        }
    }
}
