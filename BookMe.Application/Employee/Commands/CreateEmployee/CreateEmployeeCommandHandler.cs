using AutoMapper;
using BookMe.Application.Employee.Commands.CreateEmployee;
using BookMe.Domain.Entities;
using BookMe.Domain.Interfaces;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;

public class CreateEmployeeCommandHandler : IRequestHandler<CreateEmployeeCommand>
{
    private readonly IEmployeeRepository _employeeRepository;
    private readonly UserManager<ApplicationUser> _userManager;

    public CreateEmployeeCommandHandler(
        IEmployeeRepository employeeRepository,
        UserManager<ApplicationUser> userManager,
        IMapper mapper)
    {
        _employeeRepository = employeeRepository;
        _userManager = userManager;
    }

    public async Task<Unit> Handle(CreateEmployeeCommand request, CancellationToken cancellationToken)
    {
        var user = new ApplicationUser
        {
            FirstName = request.FirstName,
            LastName = request.LastName,
            Email = request.Email,
            UserName = request.Email,
            EmailConfirmed = true,
            Gender = request.Gender,
            AvatarUrl = request.AvatarUrl
        };
        
        var result = await _userManager.CreateAsync(user, request.Password);
        if (!result.Succeeded)
        {
            throw new ValidationException(result.Errors.Select(e => new FluentValidation.Results.ValidationFailure("", e.Description)));
        }
        
        var employee = new Employee
        {
            UserId = user.Id, 
            ServiceId = request.ServiceId 
        };
        
        await _employeeRepository.CreateAsync(employee);

        return Unit.Value;
    }
}
