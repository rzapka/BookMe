using BookMe.Domain.Entities;
using MediatR;

namespace BookMe.Application.Opinion.Queries.GetOpinionById
{
    public class GetOpinionByIdQuery : IRequest<Domain.Entities.Opinion>
    {
        public int Id { get; set; }
    }
}
