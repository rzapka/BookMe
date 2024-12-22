using BookMe.Application.OpeningHours.Dto;
using MediatR;
using System.Collections.Generic;

namespace BookMe.Application.OpeningHours.Queries.GetOpeningHoursByServiceId
{
    public class GetOpeningHoursByServiceIdQuery : IRequest<IEnumerable<OpeningHourDto>>
    {
        public int ServiceId { get; set; }
    }
}
