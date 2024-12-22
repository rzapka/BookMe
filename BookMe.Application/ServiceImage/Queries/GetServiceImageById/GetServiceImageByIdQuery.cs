using BookMe.Domain.Entities;
using MediatR;

namespace BookMe.Application.ServiceImage.Queries.GetServiceImageById
{
    public class GetServiceImageByIdQuery : IRequest<Domain.Entities.ServiceImage>
    {
        public int Id { get; set; }

       
    }
}
