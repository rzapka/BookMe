using MediatR;

namespace BookMe.Application.ApplicationUser.Commands.LoginApplicationUser
{
    public class LoginApplicationUserCommand : IRequest<Unit>
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
