using MediatR;
using BookMe.Application.ServiceCategory.Dto;
namespace BookMe.Application.ServiceCategory.Queries.GetAllServiceCategories
{
    public class GetAllServiceCategoriesQuery : IRequest<IEnumerable<ServiceCategoryDto>>
    {
        public string SearchTerm { get; set; } = string.Empty;
    }
}
