using MediatR;
using BookMe.Domain.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace BookMe.Application.Opinion.Commands.DeleteOpinion
{
    public class DeleteOpinionCommandHandler : IRequestHandler<DeleteOpinionCommand>
    {
        private readonly IOpinionRepository _opinionRepository;

        public DeleteOpinionCommandHandler(IOpinionRepository opinionRepository)
        {
            _opinionRepository = opinionRepository;
        }

        public async Task<Unit> Handle(DeleteOpinionCommand request, CancellationToken cancellationToken)
        {
            await _opinionRepository.DeleteOpinionAsync(request.Id);
            return Unit.Value;
        }
    }
}
