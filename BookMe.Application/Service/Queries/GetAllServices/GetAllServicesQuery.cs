using BookMe.Application.Service.Dto;
using BookMe.Domain.Entities;
using MediatR;
using System.Collections.Generic;

namespace BookMe.Application.Service.Queries.GetAllServices
{
    public class GetAllServicesQuery : IRequest<IEnumerable<ServiceDto>>
    {
    }
}
