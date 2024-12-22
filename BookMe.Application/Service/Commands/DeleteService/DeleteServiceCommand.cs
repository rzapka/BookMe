using MediatR;

namespace BookMe.Application.Service.Commands.DeleteService
{
    public class DeleteServiceCommand : IRequest<Unit>
    {
       public string EncodedName { get; set; }
    }
}
