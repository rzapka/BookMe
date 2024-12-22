using BookMe.Domain.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookMe.Application.OpeningHours.Queries.GetTakenDaysOfWeekByServiceId
{
    public class GetTakenDaysOfWeekByServiceIdQueryHandler : IRequestHandler<GetTakenDaysOfWeekByServiceIdQuery, List<string>>
    {
        private readonly IOpeningHoursRepository _openingHoursRepository;

        public GetTakenDaysOfWeekByServiceIdQueryHandler(IOpeningHoursRepository openingHoursRepository)
        {
            _openingHoursRepository = openingHoursRepository;
        }

        public async Task<List<string>> Handle(GetTakenDaysOfWeekByServiceIdQuery request, CancellationToken cancellationToken)
        {
            return await _openingHoursRepository.GetTakenDaysOfWeekByServiceId(request.ServiceId);
        }
    }
}
