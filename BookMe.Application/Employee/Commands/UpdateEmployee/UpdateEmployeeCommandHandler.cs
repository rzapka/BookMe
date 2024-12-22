using FluentValidation;
using BookMe.Application.Employee.Commands.UpdateEmployee;
using BookMe.Domain.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Identity;
using BookMe.Domain.Entities;
using BookMe.Application.Exceptions;

public class UpdateEmployeeCommandHandler : IRequestHandler<UpdateEmployeeCommand>
{
    private readonly IEmployeeRepository _employeeRepository;
    private readonly UserManager<ApplicationUser> _userManager;

    public UpdateEmployeeCommandHandler(IEmployeeRepository employeeRepository, UserManager<ApplicationUser> userManager)
    {
        _employeeRepository = employeeRepository;
        _userManager = userManager;
    }

    public async Task<Unit> Handle(UpdateEmployeeCommand request, CancellationToken cancellationToken)
    {
        var employee = await _employeeRepository.GetEmployeeByIdAsync(request.Id)
            ?? throw new KeyNotFoundException("Employee not found.");

        // Sprawdzenie, czy email jest już zajęty
        var emailConflictUser = await _userManager.FindByEmailAsync(request.Email);
        if (emailConflictUser != null && emailConflictUser.Id != employee.User.Id)
        {
            throw new UserEmailConflictException("Podany adres e-mail jest już zajęty.");
        }
        
        employee.ServiceId = request.ServiceId;
        employee.User.FirstName = request.FirstName;
        employee.User.LastName = request.LastName;
        employee.User.Gender = request.Gender;
        employee.User.Email = request.Email;
        employee.User.UserName = request.Email;
        employee.User.AvatarUrl = request.AvatarUrl;
        
        if (!string.IsNullOrWhiteSpace(request.NewPassword))
        {
            var token = await _userManager.GeneratePasswordResetTokenAsync(employee.User);
            var result = await _userManager.ResetPasswordAsync(employee.User, token, request.NewPassword);

            if (!result.Succeeded)
            {
                throw new ValidationException(result.Errors.Select(e => new FluentValidation.Results.ValidationFailure("", e.Description)));
            }
        }

        await _employeeRepository.UpdateAsync(employee);

        return Unit.Value;
    }
}
