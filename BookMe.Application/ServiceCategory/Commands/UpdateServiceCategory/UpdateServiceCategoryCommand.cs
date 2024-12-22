using BookMe.Application.Service.Dto;
using BookMe.Application.ServiceCategory.Dto;
using BookMe.Domain.Entities;
using MediatR;

namespace BookMe.Application.ServiceCategory.Commands.UpdateServiceCategory
{
    public class UpdateServiceCategoryCommand : ServiceCategoryDto, IRequest<Unit>
    {
      
    }
}
