using BookMe.Application.ServiceCategory.Dto;
using MediatR;

namespace BookMe.Application.ServiceCategory.Commands.CreateServiceCategory
{
    public class CreateServiceCategoryCommand : ServiceCategoryDto, IRequest<Unit>
    {
    
    }
}
