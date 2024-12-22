using BookMe.Application.ApplicationUser.Dto;
using MediatR;

namespace BookMe.Application.ApplicationUser.Commands.CreateApplicationUser
{
    public class CreateApplicationUserCommand : ApplicationUserDto, IRequest<Unit>
    {
    }
}
