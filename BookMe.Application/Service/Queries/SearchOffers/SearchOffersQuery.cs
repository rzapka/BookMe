using MediatR;
using System.Collections.Generic;

namespace BookMe.Application.Service.Queries.SearchOffers
{
    public class SearchOffersQuery : IRequest<List<string>>
    {
        public string Term { get; set; } = string.Empty;
    }
}
