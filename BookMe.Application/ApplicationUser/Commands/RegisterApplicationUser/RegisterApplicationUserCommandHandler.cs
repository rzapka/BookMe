using BookMe.Domain.Entities;
using BookMe.Domain.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Identity;
using FluentValidation;

namespace BookMe.Application.ApplicationUser.Commands.RegisterApplicationUser
{
    public class RegisterApplicationUserCommandHandler : IRequestHandler<RegisterApplicationUserCommand>
    {
        private readonly UserManager<Domain.Entities.ApplicationUser> _userManager;
        private readonly SignInManager<Domain.Entities.ApplicationUser> _signInManager;

        public RegisterApplicationUserCommandHandler(UserManager<Domain.Entities.ApplicationUser> userManager, SignInManager<Domain.Entities.ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<Unit> Handle(RegisterApplicationUserCommand request, CancellationToken cancellationToken)
        {
            if (request.Password != request.ConfirmPassword)
            {
                throw new ValidationException(new List<FluentValidation.Results.ValidationFailure>
                {
                    new FluentValidation.Results.ValidationFailure(nameof(request.ConfirmPassword), "Hasło i jego potwierdzenie muszą być takie same.")
                });
            }

            var user = new Domain.Entities.ApplicationUser
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                UserName = request.Email,
                IsAdmin = request.IsAdmin
            };

            var result = await _userManager.CreateAsync(user, request.Password);

            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(e => e.Description).ToList();
                throw new ValidationException(errors.Select(e =>
                    new FluentValidation.Results.ValidationFailure(nameof(request.Email), e)));
            }

            await _signInManager.SignInAsync(user, isPersistent: false);

            return Unit.Value;
        }
    }
}
