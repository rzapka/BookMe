using MediatR;

namespace BookMe.Application.OpeningHours.Commands.DeleteOpeningHour
{
    public class DeleteOpeningHourCommand : IRequest<Unit>
    {
        public int Id { get; set; }
    }
}
