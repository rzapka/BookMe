using MediatR;

namespace BookMe.Application.ServiceImage.Queries.GetServiceImagesByServiceId
{
    public class GetServiceImagesByServiceIdQuery : IRequest<List<Domain.Entities.ServiceImage>>
    {
        public int ServiceId { get; set; }
    }
}
