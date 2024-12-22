using AutoMapper;
using BookMe.Application.Employee.Dto;
using BookMe.Domain.Entities;
using BookMe.Domain.Interfaces;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace BookMe.Application.Employee.Queries.GetEmployeesByServiceId
{
    public class GetEmployeesByServiceIdQueryHandler : IRequestHandler<GetEmployeesByServiceIdQuery, IEnumerable<EmployeeDto>>
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IMapper _mapper;

        public GetEmployeesByServiceIdQueryHandler(IEmployeeRepository employeeRepository, IMapper mapper)
        {
            _employeeRepository = employeeRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<EmployeeDto>> Handle(GetEmployeesByServiceIdQuery request, CancellationToken cancellationToken)
        {
            var employees = await _employeeRepository.GetEmployeesByServiceIdAsync(request.ServiceId);
            var dtos = _mapper.Map<IEnumerable<EmployeeDto>>(employees);
            return dtos;
        }
    }
}
