using AutoMapper;
using BookMe.Application.OpeningHours.Dto;
using BookMe.Domain.Interfaces;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace BookMe.Application.OpeningHours.Queries.GetOpeningHoursByServiceId
{
    public class GetOpeningHoursByServiceIdQueryHandler : IRequestHandler<GetOpeningHoursByServiceIdQuery, IEnumerable<OpeningHourDto>>
    {
        private readonly IOpeningHoursRepository _openingHoursRepository;
        private readonly IMapper _mapper;

        public GetOpeningHoursByServiceIdQueryHandler(IOpeningHoursRepository openingHoursRepository, IMapper mapper)
        {
            _openingHoursRepository = openingHoursRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<OpeningHourDto>> Handle(GetOpeningHoursByServiceIdQuery request, CancellationToken cancellationToken)
        {
            var openingHours = await _openingHoursRepository.GetByServiceIdAsync(request.ServiceId);
            return _mapper.Map<IEnumerable<OpeningHourDto>>(openingHours);
        }
    }
}
