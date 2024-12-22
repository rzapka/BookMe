using MediatR;

namespace BookMe.Application.ServiceImage.Commands.DeleteServiceImage
{
    public class DeleteServiceImageCommand : IRequest<Unit>
    {
        public int Id { get; set; }
    }
}
