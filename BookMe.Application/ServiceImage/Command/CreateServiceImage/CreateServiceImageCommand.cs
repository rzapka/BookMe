using MediatR;

namespace BookMe.Application.ServiceImage.Commands.CreateServiceImage
{
    public class CreateServiceImageCommand : IRequest<Unit>
    {
        public string Url { get; set; } = default!;
        public int ServiceId { get; set; }
    }
}
