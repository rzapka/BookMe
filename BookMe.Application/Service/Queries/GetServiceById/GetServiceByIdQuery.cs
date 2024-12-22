using BookMe.Application.Service.Dto;
using BookMe.Domain.Entities;
using MediatR;

namespace BookMe.Application.Service.Queries.GetServiceById
{
    public class GetServiceByIdQuery : IRequest<ServiceDto>
    {
        public int Id { get; set; }
    }
}
