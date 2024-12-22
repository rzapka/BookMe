using BookMe.Application.Employee.Dto;
using BookMe.Domain.Entities;
using MediatR;
using System.Collections.Generic;

namespace BookMe.Application.Employee.Queries.GetEmployeesByServiceEncodedName
{
    public class GetEmployeesByServiceEncodedNameQuery : IRequest<IEnumerable<EmployeeDto>>
    {
        public string ServiceEncodedName { get; set; } = string.Empty;
    }
}