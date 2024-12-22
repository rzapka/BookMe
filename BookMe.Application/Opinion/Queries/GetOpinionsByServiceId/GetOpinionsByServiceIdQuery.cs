using BookMe.Application.Opinion.Dto;
using BookMe.Domain.Entities;
using MediatR;
using System.Collections.Generic;

namespace BookMe.Application.Opinion.Queries.GetOpinionsByServiceId
{
    public class GetOpinionsByServiceIdQuery : IRequest<IEnumerable<OpinionDto>>
    {
        public int ServiceId { get; set; }
    }
}
