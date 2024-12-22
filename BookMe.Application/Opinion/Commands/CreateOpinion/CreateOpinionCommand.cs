using MediatR;

namespace BookMe.Application.Opinion.Commands.CreateOpinion
{
    public class CreateOpinionCommand : IRequest<Unit>
    {
        public int ServiceId { get; set; }

        public string ServiceEncodedName { get; set; } = string.Empty;
        public int OfferId { get; set; }
        public int EmployeeId { get; set; }
        public string UserId { get; set; } = string.Empty;
        public int BookingId { get; set; }
        public int Rating { get; set; }
        public string Content { get; set; } = string.Empty;
    }
}