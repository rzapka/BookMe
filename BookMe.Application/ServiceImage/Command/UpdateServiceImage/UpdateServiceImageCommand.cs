using MediatR;
using System.ComponentModel.DataAnnotations;

namespace BookMe.Application.ServiceImage.Commands.UpdateServiceImage
{
    public class UpdateServiceImageCommand : IRequest<Unit>
    {
        public int Id { get; set; }

        public string Url { get; set; } = default!;
    }
}
