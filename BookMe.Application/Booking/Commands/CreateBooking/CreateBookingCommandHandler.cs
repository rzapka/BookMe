using BookMe.Domain.Entities;
using BookMe.Domain.Interfaces;
using FluentValidation;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace BookMe.Application.Booking.Commands.CreateBooking
{
    public class CreateBookingCommandHandler : IRequestHandler<CreateBookingCommand>
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly IOfferRepository _offerRepository;
        private readonly IValidator<CreateBookingCommand> _validator;

        public CreateBookingCommandHandler(IBookingRepository bookingRepository, IOfferRepository offerRepository, IValidator<CreateBookingCommand> validator)
        {
            _bookingRepository = bookingRepository;
            _offerRepository = offerRepository;
            _validator = validator;
        }

        public async Task<Unit> Handle(CreateBookingCommand request, CancellationToken cancellationToken)
        {
            var offer = await _offerRepository.GetByIdAsync(request.OfferId);
            if (offer == null)
            {
                throw new ValidationException("Invalid offer selected.");
            }

            request.Offer = offer;
            request.SetEndTime();

            var validationResult = await _validator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            var booking = new Domain.Entities.Booking
            {
                EmployeeId = request.EmployeeId,
                OfferId = request.OfferId,
                UserId = request.UserId,
                StartTime = request.StartTime,
                ServiceId = offer.ServiceId,
            };
            booking.SetEndTime();
            await _bookingRepository.Create(booking);
            return Unit.Value;
        }
    }
}