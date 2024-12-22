using BookMe.Domain.Entities;
using BookMe.Domain.Interfaces;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace BookMe.Application.OpeningHours.Commands.CreateOpeningHour
{
    public class CreateOpeningHourCommandHandler : IRequestHandler<CreateOpeningHourCommand, Unit>
    {
        private readonly IOpeningHoursRepository _openingHoursRepository;

        public CreateOpeningHourCommandHandler(IOpeningHoursRepository openingHoursRepository)
        {
            _openingHoursRepository = openingHoursRepository;
        }

        public async Task<Unit> Handle(CreateOpeningHourCommand request, CancellationToken cancellationToken)
        {
            var openingHour = new OpeningHour
            {
                DayOfWeek = request.DayOfWeek,
                OpeningTime = request.OpeningTime,
                ClosingTime = request.ClosingTime,
                Closed = request.Closed,
                ServiceId = request.ServiceId
            };

            await _openingHoursRepository.AddAsync(openingHour);
            return Unit.Value; // Successfully processed
        }
    }
}
