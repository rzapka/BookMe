using BookMe.Application.Employee.Dto;
using MediatR;

namespace BookMe.Application.Employee.Queries.GetEmployeeByUserId
{
    public class GetEmployeeByUserIdQuery : IRequest<EmployeeDto>
    {
        public string UserId { get; set; }
    }
}
