using BookMe.Domain.Interfaces;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace BookMe.Application.OpeningHours.Commands.UpdateOpeningHour
{
    public class UpdateOpeningHourCommandHandler : IRequestHandler<UpdateOpeningHourCommand>
    {
        private readonly IOpeningHoursRepository _openingHoursRepository;

        public UpdateOpeningHourCommandHandler(IOpeningHoursRepository openingHoursRepository)
        {
            _openingHoursRepository = openingHoursRepository;
        }

        public async Task<Unit> Handle(UpdateOpeningHourCommand request, CancellationToken cancellationToken)
        {

            var openingHour = await _openingHoursRepository.GetByIdAsync(request.Id);
            if (openingHour == null)
            {
                throw new KeyNotFoundException("Opening hour not found.");
            }

            openingHour.DayOfWeek = request.DayOfWeek;
            openingHour.OpeningTime = request.OpeningTime;
            openingHour.ClosingTime = request.ClosingTime;
            openingHour.Closed = request.Closed;
            openingHour.ServiceId = request.ServiceId;

            await _openingHoursRepository.UpdateAsync(openingHour);
            return Unit.Value;
        }
    }
}
