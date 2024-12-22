using BookMe.Application.ServiceCategory.Dto;
using BookMe.Domain.Entities;
using MediatR;

namespace BookMe.Application.ServiceCategory.Queries.GetServiceCategoryByEncodedName
{
    public class GetServiceCategoryByEncodedNameQuery : IRequest<ServiceCategoryDto>
    {
        public string EncodedName { get; }
        public string SearchTerm { get; }

        public GetServiceCategoryByEncodedNameQuery(string encodedName, string searchTerm = "")
        {
            EncodedName = encodedName;
            SearchTerm = searchTerm;
        }
    }
}
