using AutoMapper;
using BookMe.Application.Opinion.Dto;
using BookMe.Domain.Entities;
using BookMe.Domain.Interfaces;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace BookMe.Application.Opinion.Queries.GetOpinionsByServiceId
{
    public class GetOpinionsByServiceIdQueryHandler : IRequestHandler<GetOpinionsByServiceIdQuery, IEnumerable<OpinionDto>>
    {
        private readonly IOpinionRepository _opinionRepository;
        private readonly IMapper _mapper;

        public GetOpinionsByServiceIdQueryHandler(IOpinionRepository opinionRepository, IMapper mapper)
        {
            _opinionRepository = opinionRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<OpinionDto>> Handle(GetOpinionsByServiceIdQuery request, CancellationToken cancellationToken)
        {
            var services = await _opinionRepository.GetOpinionsByServiceIdAsync(request.ServiceId);
            var dtos = _mapper.Map<IEnumerable<OpinionDto>>(services);
            return dtos;
        }
    }
}
