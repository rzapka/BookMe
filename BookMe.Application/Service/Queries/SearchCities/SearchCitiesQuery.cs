using MediatR;
using System.Collections.Generic;

namespace BookMe.Application.Service.Queries.SearchCities
{
    public class SearchCitiesQuery : IRequest<List<string>>
    {
        public string Term { get; set; } = string.Empty;
    }
}
