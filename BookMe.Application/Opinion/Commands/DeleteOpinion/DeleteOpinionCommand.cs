using MediatR;

namespace BookMe.Application.Opinion.Commands.DeleteOpinion
{
    public class DeleteOpinionCommand : IRequest<Unit>
    {
        public int Id { get; set; }
    }
}
