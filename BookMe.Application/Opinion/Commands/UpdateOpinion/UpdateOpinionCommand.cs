using BookMe.Application.Opinion.Dto;
using MediatR;

namespace BookMe.Application.Opinion.Commands.UpdateOpinion
{
    public class UpdateOpinionCommand : OpinionDto, IRequest<Unit>
    {
        public int EmployeeId { get; set; }
        public string UserId { get; set; }
        public int BookingId { get; set; }
        public int OfferId { get; set; }
        public int ServiceId { get; set; }
        public string ServiceName { get; set; }
        public string ServiceEncodedName { get; set; }
        public DateTime BookingStartTime { get; set; }
        public int OfferDuration { get; set; }
        public decimal OfferPrice { get; set; }
    }
}
