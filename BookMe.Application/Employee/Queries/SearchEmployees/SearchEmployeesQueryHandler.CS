using AutoMapper;
using BookMe.Application.Employee.Dto;
using BookMe.Domain.Interfaces;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace BookMe.Application.Employee.Queries.SearchEmployees
{
    public class SearchEmployeesQueryHandler : IRequestHandler<SearchEmployeesQuery, IEnumerable<EmployeeDto>>
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IMapper _mapper;

        public SearchEmployeesQueryHandler(IEmployeeRepository employeeRepository, IMapper mapper)
        {
            _employeeRepository = employeeRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<EmployeeDto>> Handle(SearchEmployeesQuery request, CancellationToken cancellationToken)
        {
            var employees = await _employeeRepository.SearchEmployeesAsync(request.SearchTerm);
            return _mapper.Map<IEnumerable<EmployeeDto>>(employees);
        }
    }
}
