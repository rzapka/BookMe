using BookMe.Application.ApplicationUser.Dto;
using MediatR;

namespace BookMe.Application.ApplicationUser.Commands.RegisterApplicationUser
{
    public class RegisterApplicationUserCommand : ApplicationUserDto, IRequest<Unit>
    {
    }
}
