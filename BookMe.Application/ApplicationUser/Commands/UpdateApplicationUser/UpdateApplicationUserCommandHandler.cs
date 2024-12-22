using BookMe.Application.ApplicationUser.Commands.UpdateApplicationUser;
using BookMe.Domain.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Identity;
using FluentValidation;
using BookMe.Application.Exceptions;

namespace BookMe.Application.ApplicationUser.Commands
{
    public class UpdateApplicationUserCommandHandler : IRequestHandler<UpdateApplicationUserCommand>
    {
        private readonly IApplicationUserRepository _userRepository;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly UserManager<Domain.Entities.ApplicationUser> _userManager;

        public UpdateApplicationUserCommandHandler(
            IApplicationUserRepository userRepository,
            IEmployeeRepository employeeRepository,
            UserManager<Domain.Entities.ApplicationUser> userManager)
        {
            _userRepository = userRepository;
            _employeeRepository = employeeRepository;
            _userManager = userManager;
        }

        public async Task<Unit> Handle(UpdateApplicationUserCommand request, CancellationToken cancellationToken)
        {
            var existingUser = await _userRepository.GetByIdAsync(request.Id);
            if (existingUser == null)
            {
                throw new KeyNotFoundException("Użytkownik nie został znaleziony.");
            }

            var emailExists = await _userManager.FindByEmailAsync(request.Email);
            if (emailExists != null && emailExists.Id != existingUser.Id)
            {
                throw new UserEmailConflictException("Podany adres e-mail jest już zajęty.");
            }

            // Aktualizacja danych użytkownika
            existingUser.FirstName = request.FirstName;
            existingUser.LastName = request.LastName;
            existingUser.Email = request.Email;
            existingUser.UserName = request.Email;
            existingUser.Gender = request.Gender;
            existingUser.AvatarUrl = request.AvatarUrl;
            existingUser.IsAdmin = request.IsAdmin;

            if (!string.IsNullOrEmpty(request.NewPassword))
            {
                var token = await _userManager.GeneratePasswordResetTokenAsync(existingUser);
                var resetResult = await _userManager.ResetPasswordAsync(existingUser, token, request.NewPassword);
                if (!resetResult.Succeeded)
                {
                    var errors = string.Join(", ", resetResult.Errors.Select(e => e.Description));
                    throw new ValidationException($"Błąd przy aktualizacji hasła: {errors}");
                }
            }

            await _userRepository.UpdateAsync(existingUser);

            if (request.ServiceId.HasValue)
            {
                var employee = new Domain.Entities.Employee
                {
                    UserId = existingUser.Id,
                    ServiceId = request.ServiceId.Value
                };
                await _employeeRepository.CreateAsync(employee);
            }

            // Dodanie do roli Admin jeśli IsAdmin = true
            if (request.IsAdmin)
            {
                // Sprawdź czy użytkownik nie jest już w roli Admin
                var roles = await _userManager.GetRolesAsync(existingUser);
                if (!roles.Contains("Admin"))
                {
                    var addToRoleResult = await _userManager.AddToRoleAsync(existingUser, "Admin");
                    if (!addToRoleResult.Succeeded)
                    {
                        var errors = string.Join(", ", addToRoleResult.Errors.Select(e => e.Description));
                        throw new ValidationException($"Błąd przy dodawaniu roli Admin: {errors}");
                    }
                }
            }

            return Unit.Value;
        }

    }
}
