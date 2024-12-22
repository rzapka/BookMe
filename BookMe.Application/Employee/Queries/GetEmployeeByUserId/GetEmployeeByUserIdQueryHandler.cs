using AutoMapper;
using BookMe.Application.Employee.Dto;
using BookMe.Domain.Interfaces;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace BookMe.Application.Employee.Queries.GetEmployeeByUserId
{
    public class GetEmployeeByUserIdQueryHandler : IRequestHandler<GetEmployeeByUserIdQuery, EmployeeDto>
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IMapper _mapper;

        public GetEmployeeByUserIdQueryHandler(IEmployeeRepository employeeRepository, IMapper mapper)
        {
            _employeeRepository = employeeRepository;
            _mapper = mapper;
        }

        public async Task<EmployeeDto> Handle(GetEmployeeByUserIdQuery request, CancellationToken cancellationToken)
        {
            var employee = await _employeeRepository.GetEmployeeByUserIdAsync(request.UserId);
            return _mapper.Map<EmployeeDto>(employee);
        }
    }
}
