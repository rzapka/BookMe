using MediatR;
using System;

namespace BookMe.Application.Booking.Commands.CreateBooking
{
    public class CreateBookingCommand : IRequest<Unit>
    {
        public int EmployeeId { get; set; }
        public int OfferId { get; set; }
        public string UserId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }

        public Domain.Entities.Offer Offer { get; set; }

        public void SetEndTime()
        {
            if (Offer != null)
            {
                EndTime = StartTime.AddMinutes(Offer.Duration);
            }
        }
    }
}