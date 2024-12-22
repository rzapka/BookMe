using BookMe.Domain.Entities;
using BookMe.Domain.Interfaces;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace BookMe.Application.Opinion.Commands.CreateOpinion
{
    public class CreateOpinionCommandHandler : IRequestHandler<CreateOpinionCommand>
    {
        private readonly IOpinionRepository _opinionRepository;

        public CreateOpinionCommandHandler(IOpinionRepository opinionRepository)
        {
            _opinionRepository = opinionRepository;
        }

        public async Task<Unit> Handle(CreateOpinionCommand request, CancellationToken cancellationToken)
        {
            var opinion = new Domain.Entities.Opinion
            {
                ServiceId = request.ServiceId,
                OfferId = request.OfferId,
                EmployeeId = request.EmployeeId,
                UserId = request.UserId,
                BookingId = request.BookingId,
                Rating = request.Rating,
                Content = request.Content
            };

            await _opinionRepository.CreateOpinionAsync(opinion);
            return Unit.Value;
        }
    }
}
