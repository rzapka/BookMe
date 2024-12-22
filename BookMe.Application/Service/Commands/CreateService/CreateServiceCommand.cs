using BookMe.Application.Service.Dto;
using BookMe.Domain.Entities;
using MediatR;

namespace BookMe.Application.Service.Commands.CreateService
{
    public class CreateServiceCommand : ServiceDto, IRequest<Unit>
    {
    }
}