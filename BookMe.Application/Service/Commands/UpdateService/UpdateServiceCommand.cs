using BookMe.Application.Service.Dto;
using BookMe.Domain.Entities;
using MediatR;

namespace BookMe.Application.Service.Commands.UpdateService
{
    public class UpdateServiceCommand : ServiceDto, IRequest<Unit>
    {
    }
}
