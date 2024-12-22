using BookMe.Application.Employee.Commands.UpdateEmployee;
using BookMe.Application.Employee.Dto;
using MediatR;

namespace BookMe.Application.Employee.Queries.GetEmployeeById
{
    public class GetEmployeeByIdQuery : IRequest<UpdateEmployeeCommand>
    {
        public int Id { get; set; }
    }
}
