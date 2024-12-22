using MediatR;
using FluentValidation.Results;
using BookMe.Application.OpeningHours.Dto;

namespace BookMe.Application.OpeningHours.Commands.UpdateOpeningHour
{
    public class UpdateOpeningHourCommand : OpeningHourDto, IRequest<Unit>
    {

    }
}
