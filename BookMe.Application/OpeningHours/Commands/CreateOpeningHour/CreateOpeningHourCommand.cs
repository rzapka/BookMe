using MediatR;
using FluentValidation.Results;

namespace BookMe.Application.OpeningHours.Commands.CreateOpeningHour
{
    public class CreateOpeningHourCommand : IRequest<Unit>
    {
        public string DayOfWeek { get; set; } = string.Empty;
        public TimeSpan OpeningTime { get; set; }
        public TimeSpan ClosingTime { get; set; }
        public bool Closed { get; set; }
        public int ServiceId { get; set; }
    }
}
