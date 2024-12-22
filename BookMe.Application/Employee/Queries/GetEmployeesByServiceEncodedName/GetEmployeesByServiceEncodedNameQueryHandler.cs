using AutoMapper;
using BookMe.Application.Employee.Dto;
using BookMe.Domain.Entities;
using BookMe.Domain.Interfaces;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace BookMe.Application.Employee.Queries.GetEmployeesByServiceEncodedName
{
    public class GetEmployeesByServiceIdQueryHandler : IRequestHandler<GetEmployeesByServiceEncodedNameQuery, IEnumerable<EmployeeDto>>
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IMapper _mapper;

        public GetEmployeesByServiceIdQueryHandler(IEmployeeRepository employeeRepository, IMapper mapper)
        {
            _employeeRepository = employeeRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<EmployeeDto>> Handle(GetEmployeesByServiceEncodedNameQuery request, CancellationToken cancellationToken)
        {
            var employees = await _employeeRepository.GetEmployeesByServiceEncodedNameAsync(request.ServiceEncodedName);
            var dtos = _mapper.Map<IEnumerable<EmployeeDto>>(employees);
            return dtos;
        }
    }
}
