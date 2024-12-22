using BookMe.Application.Employee.Dto;
using BookMe.Domain.Entities;
using MediatR;
using System.Collections.Generic;

namespace BookMe.Application.Employee.Queries.GetEmployeesByServiceId
{
    public class GetEmployeesByServiceIdQuery : IRequest<IEnumerable<EmployeeDto>>
    {
        public int ServiceId { get; set; }
    }
}