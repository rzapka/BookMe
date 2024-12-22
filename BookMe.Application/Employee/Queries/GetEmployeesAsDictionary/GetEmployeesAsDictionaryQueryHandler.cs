using AutoMapper;
using BookMe.Domain.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookMe.Application.Employee.Queries.GetEmployeesAsDictionary
{
    public class GetEmployeesAsDictionaryQueryHandler : IRequestHandler<GetEmployeesAsDictionaryQuery, Dictionary<int, string>>
    {

        private readonly IEmployeeRepository _employeeRepository;

        public GetEmployeesAsDictionaryQueryHandler(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }


        public Task<Dictionary<int, string>> Handle(GetEmployeesAsDictionaryQuery request, CancellationToken cancellationToken)
        {
            return _employeeRepository.GetEmployeesByServiceEncodedNameAsDictionary(request.ServiceEncodedName);
        }
    }
}
