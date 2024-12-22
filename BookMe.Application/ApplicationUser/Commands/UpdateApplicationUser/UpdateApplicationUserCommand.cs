using BookMe.Application.ApplicationUser.Dto;
using BookMe.Domain.Enums;
using MediatR;

namespace BookMe.Application.ApplicationUser.Commands.UpdateApplicationUser
{
    public class UpdateApplicationUserCommand : ApplicationUserDto, IRequest<Unit>
    {
        public int? ServiceId { get; set; } 
    }
}
