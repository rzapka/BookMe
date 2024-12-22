using AutoMapper;
using BookMe.Application.Opinion.Dto;
using BookMe.Domain.Interfaces;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BookMe.Application.Opinion.Queries.GetOpinionsByServiceEncodedName
{
    public class GetOpinionsByServiceEncodedNameQueryHandler : IRequestHandler<GetOpinionsByServiceEncodedNameQuery, IEnumerable<OpinionDto>>
    {
        private readonly IOpinionRepository _opinionRepository;
        private readonly IMapper _mapper;

        public GetOpinionsByServiceEncodedNameQueryHandler(IOpinionRepository opinionRepository, IMapper mapper)
        {
            _opinionRepository = opinionRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<OpinionDto>> Handle(GetOpinionsByServiceEncodedNameQuery request, CancellationToken cancellationToken)
        {
            var opinions = await _opinionRepository.GetOpinionsByServiceEncodedNameAsync(request.EncodedName);
            var dtos = _mapper.Map<IEnumerable<OpinionDto>>(opinions);
            return dtos;
        }
    }
}
