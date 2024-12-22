using BookMe.Application.OpeningHours.Dto;
using MediatR;

namespace BookMe.Application.OpeningHours.Queries.GetOpeningHoursByServicedEncodedName
{
    public class GetOpeningHoursByServiceEncodedNameQuery : IRequest<List<OpeningHourDto>>
    {
        public string EncodedName { get; set; } = default!;
    }
}
