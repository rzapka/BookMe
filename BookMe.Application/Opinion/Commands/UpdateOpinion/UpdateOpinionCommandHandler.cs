using AutoMapper;
using BookMe.Application.Opinion.Dto;
using BookMe.Domain.Interfaces;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace BookMe.Application.Opinion.Commands.UpdateOpinion
{
    public class UpdateOpinionCommandHandler : IRequestHandler<UpdateOpinionCommand>
    {
        private readonly IOpinionRepository _opinionRepository;

        public UpdateOpinionCommandHandler(IOpinionRepository opinionRepository)
        {
            _opinionRepository = opinionRepository;
        }

        public async Task<Unit> Handle(UpdateOpinionCommand request, CancellationToken cancellationToken)
        {
            var opinion = await _opinionRepository.GetOpinionByIdAsync(request.Id);
            if (opinion == null)
            {
                throw new KeyNotFoundException("Opinion not found.");
            }

            opinion.Content = request.Content;
            opinion.Rating = request.Rating;
            opinion.EmployeeId = request.EmployeeId;
            opinion.OfferId = request.OfferId; 

            await _opinionRepository.Commit();

            return Unit.Value;
        }
    }
}
