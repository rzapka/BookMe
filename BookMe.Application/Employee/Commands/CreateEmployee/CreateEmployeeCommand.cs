using BookMe.Application.Employee.Dto;
using MediatR;

namespace BookMe.Application.Employee.Commands.CreateEmployee
{
    public class CreateEmployeeCommand : EmployeeDto, IRequest<Unit>
    {
        public string ServiceEncodedName { get; set; } = string.Empty;
    }
}
