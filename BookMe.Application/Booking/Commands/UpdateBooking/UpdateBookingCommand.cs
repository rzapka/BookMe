using BookMe.Application.Booking.Dto;
using BookMe.Domain.Entities;
using MediatR;
using System;

namespace BookMe.Application.Booking.Commands.UpdateBooking
{
    public class UpdateBookingCommand : BookingDto, IRequest<Unit>
    {
        public Domain.Entities.Offer Offer { get; set; }
        public string ServiceEncodedName { get; set; }
        public string OfferEncodedName { get; set; }


        public void SetEndTime()
        {
            if (Offer != null)
            {
                EndTime = StartTime.AddMinutes(Offer.Duration);
            }
        }
    }
}
