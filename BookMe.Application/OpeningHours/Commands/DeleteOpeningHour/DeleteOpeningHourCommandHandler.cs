using BookMe.Domain.Interfaces;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace BookMe.Application.OpeningHours.Commands.DeleteOpeningHour
{
    public class DeleteOpeningHourCommandHandler : IRequestHandler<DeleteOpeningHourCommand>
    {
        private readonly IOpeningHoursRepository _openingHoursRepository;

        public DeleteOpeningHourCommandHandler(IOpeningHoursRepository openingHoursRepository)
        {
            _openingHoursRepository = openingHoursRepository;
        }

        public async Task<Unit> Handle(DeleteOpeningHourCommand request, CancellationToken cancellationToken)
        {
            var openingHour = await _openingHoursRepository.GetByIdAsync(request.Id);
            if (openingHour == null)
            {
                throw new KeyNotFoundException("Opening hour not found.");
            }

            await _openingHoursRepository.DeleteAsync(request.Id);
            return Unit.Value;
        }
    }
}
