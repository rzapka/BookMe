using BookMe.Application.Offer.Dto;
using BookMe.Domain.Entities;
using MediatR;
using System.Collections.Generic;

namespace BookMe.Application.Booking.Queries.GetBookingCreationData
{
    public class GetBookingCreationDataQuery : IRequest<BookingCreationDataResult>
    {
        public string ServiceEncodedName { get; set; }
        public string OfferEncodedName { get; set; }
    }

    public class BookingCreationDataResult
    {
        public Dictionary<int, string> Employees { get; set; }
        public OfferDto Offer { get; set; }
    }
}
