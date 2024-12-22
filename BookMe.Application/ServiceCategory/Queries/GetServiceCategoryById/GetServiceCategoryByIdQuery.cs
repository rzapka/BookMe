using BookMe.Application.ServiceCategory.Commands.UpdateServiceCategory;
using BookMe.Application.ServiceCategory.Dto;
using BookMe.Domain.Entities;
using MediatR;

namespace BookMe.Application.ServiceCategory.Queries.GetServiceCategoryById
{
    public class GetServiceCategoryByIdQuery :  IRequest<UpdateServiceCategoryCommand>
    {
        public int Id { get; set; }
    }
}
