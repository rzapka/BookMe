using BookMe.Domain.Entities;
using BookMe.Domain.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Identity;
using FluentValidation;

namespace BookMe.Application.ApplicationUser.Commands.CreateApplicationUser
{
    public class CreateApplicationUserCommandHandler : IRequestHandler<CreateApplicationUserCommand>
    {
        private readonly UserManager<Domain.Entities.ApplicationUser> _userManager;

        public CreateApplicationUserCommandHandler(UserManager<Domain.Entities.ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<Unit> Handle(CreateApplicationUserCommand request, CancellationToken cancellationToken)
        {
            var user = new Domain.Entities.ApplicationUser
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                UserName = request.Email,
                EmailConfirmed = true,
                IsAdmin = request.IsAdmin
            };

            var result = await _userManager.CreateAsync(user, request.Password);
   
            if (!result.Succeeded)
            {
                throw new ValidationException(result.Errors.Select(e => new FluentValidation.Results.ValidationFailure("", e.Description)));
            }

            if (request.IsAdmin)
            {
                var addToRoleResult = await _userManager.AddToRoleAsync(user, "Admin");
                if (!addToRoleResult.Succeeded)
                {
                    var errors = string.Join(", ", addToRoleResult.Errors.Select(e => e.Description));
                    throw new ValidationException($"Błąd przy dodawaniu roli Admin: {errors}");
                }
            }

            return Unit.Value;
        }
    }
}