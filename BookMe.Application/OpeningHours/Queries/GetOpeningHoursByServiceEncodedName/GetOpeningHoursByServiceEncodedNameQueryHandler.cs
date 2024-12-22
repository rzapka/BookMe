using AutoMapper;
using BookMe.Application.OpeningHours.Dto;
using BookMe.Domain.Interfaces;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace BookMe.Application.OpeningHours.Queries.GetOpeningHoursByServicedEncodedName
{
    public class GetOpeningHoursByServiceEncodedNameQueryHandler : IRequestHandler<GetOpeningHoursByServiceEncodedNameQuery, List<OpeningHourDto>>
    {
        private readonly IOpeningHoursRepository _openingHoursRepository;
        private readonly IMapper _mapper;

        public GetOpeningHoursByServiceEncodedNameQueryHandler(IOpeningHoursRepository openingHoursRepository, IMapper mapper)
        {
            _openingHoursRepository = openingHoursRepository;
            _mapper = mapper;
        }

        public async Task<List<OpeningHourDto>> Handle(GetOpeningHoursByServiceEncodedNameQuery request, CancellationToken cancellationToken)
        {
            var openingHours = await _openingHoursRepository.GetOpeningHoursByServiceEncodedNameAsync(request.EncodedName);
            return _mapper.Map<List<OpeningHourDto>>(openingHours);
        }
    }
}
