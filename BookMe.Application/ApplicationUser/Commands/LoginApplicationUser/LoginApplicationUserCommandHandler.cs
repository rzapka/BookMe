using MediatR;
using Microsoft.AspNetCore.Identity;
using BookMe.Domain.Entities;
using FluentValidation;

namespace BookMe.Application.ApplicationUser.Commands.LoginApplicationUser
{
    public class LoginApplicationUserCommandHandler : IRequestHandler<LoginApplicationUserCommand>
    {
        private readonly SignInManager<Domain.Entities.ApplicationUser> _signInManager;

        public LoginApplicationUserCommandHandler(SignInManager<Domain.Entities.ApplicationUser> signInManager)
        {
            _signInManager = signInManager;
        }

        public async Task<Unit> Handle(LoginApplicationUserCommand request, CancellationToken cancellationToken)
        {
            var result = await _signInManager.PasswordSignInAsync(request.Email, request.Password, isPersistent: false, lockoutOnFailure: false);

            if (!result.Succeeded)
            {
                Console.WriteLine("Login failed for email: " + request.Email);
                throw new ValidationException(new List<FluentValidation.Results.ValidationFailure>
                {
                    new FluentValidation.Results.ValidationFailure("", "Nieprawidłowy email lub hasło.")
                });
            }


            return Unit.Value;
        }
    }
}
