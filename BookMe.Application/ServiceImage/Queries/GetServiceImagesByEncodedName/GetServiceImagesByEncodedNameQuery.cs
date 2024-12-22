using BookMe.Domain.Entities;
using MediatR;

namespace BookMe.Application.ServiceImage.Queries.GetServiceImagesByEncodedName
{
    public class GetServiceImagesByEncodedNameQuery : IRequest<List<Domain.Entities.ServiceImage>>
    {
        public string EncodedName { get; set; } = default!;
    }
}
