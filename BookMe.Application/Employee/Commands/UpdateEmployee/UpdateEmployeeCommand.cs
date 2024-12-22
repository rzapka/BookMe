using BookMe.Application.Employee.Dto;
using BookMe.Domain.Enums;
using MediatR;

namespace BookMe.Application.Employee.Commands.UpdateEmployee
{
    public class UpdateEmployeeCommand : EmployeeDto, IRequest<Unit>
    {

        public string? NewPassword { get; set; }

        public string ServiceEncodedName { get; set; } = string.Empty;
    }
}
