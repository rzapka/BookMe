using MediatR;
using System.Collections.Generic;
using BookMe.Domain.Entities;
using BookMe.Application.Service.Dto;

namespace BookMe.Application.Service.Queries.GetRecommendedServices
{
    public class GetRecommendedServicesQuery : IRequest<List<ServiceDto>>
    {
    }
}
