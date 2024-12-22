using BookMe.Application.Employee.Dto;
using BookMe.Domain.Entities;
using MediatR;
using System.Collections.Generic;

namespace BookMe.Application.Employee.Queries.SearchEmployees
{
    public class SearchEmployeesQuery : EmployeeDto,  IRequest<IEnumerable<EmployeeDto>>
    {
        public string SearchTerm { get; set; } = string.Empty;
    }
}
