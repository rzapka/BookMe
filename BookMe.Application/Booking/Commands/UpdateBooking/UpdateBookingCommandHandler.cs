using BookMe.Domain.Interfaces;
using FluentValidation;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace BookMe.Application.Booking.Commands.UpdateBooking
{
    public class UpdateBookingCommandHandler : IRequestHandler<UpdateBookingCommand>
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly IValidator<UpdateBookingCommand> _validator;

        public UpdateBookingCommandHandler(IBookingRepository bookingRepository, IValidator<UpdateBookingCommand> validator)
        {
            _bookingRepository = bookingRepository;
            _validator = validator;
        }

        public async Task<Unit> Handle(UpdateBookingCommand request, CancellationToken cancellationToken)
        {
            var booking = await _bookingRepository.GetBookingByIdAsync(request.Id);

            if (booking == null)
            {
                throw new ValidationException("Nie znaleziono rezerwacji.");
            }

            var validationResult = await _validator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            booking.StartTime = request.StartTime;
            booking.EmployeeId = request.EmployeeId;
            booking.SetEndTime();

            await _bookingRepository.UpdateBookingAsync(booking);

            return Unit.Value;
        }
    }
}
