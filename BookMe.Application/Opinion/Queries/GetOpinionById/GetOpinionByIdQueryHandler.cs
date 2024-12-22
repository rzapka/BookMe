using BookMe.Domain.Entities;
using BookMe.Domain.Interfaces;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace BookMe.Application.Opinion.Queries.GetOpinionById
{
    public class GetOpinionByIdQueryHandler : IRequestHandler<GetOpinionByIdQuery, Domain.Entities.Opinion>
    {
        private readonly IOpinionRepository _opinionRepository;

        public GetOpinionByIdQueryHandler(IOpinionRepository opinionRepository)
        {
            _opinionRepository = opinionRepository;
        }

        public async Task<Domain.Entities.Opinion> Handle(GetOpinionByIdQuery request, CancellationToken cancellationToken)
        {
            return await _opinionRepository.GetOpinionByIdAsync(request.Id);
        }
    }
}
