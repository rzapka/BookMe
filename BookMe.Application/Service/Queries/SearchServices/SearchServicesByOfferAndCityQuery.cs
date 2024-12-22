using BookMe.Domain.Entities;
using MediatR;
using System.Collections.Generic;

namespace BookMe.Application.Service.Queries.SearchServices
{
    public class SearchServicesByOfferAndCityQuery : IRequest<List<Domain.Entities.Service>>
    {
        public string SearchTerm { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
    }
}
