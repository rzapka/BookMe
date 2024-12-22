using BookMe.Application.Opinion.Dto;
using MediatR;
using System.Collections.Generic;

namespace BookMe.Application.Opinion.Queries.GetOpinionsByServiceEncodedName
{
    public class GetOpinionsByServiceEncodedNameQuery : IRequest<IEnumerable<OpinionDto>>
    {
        public string EncodedName { get; set; } = string.Empty;
    }
}
