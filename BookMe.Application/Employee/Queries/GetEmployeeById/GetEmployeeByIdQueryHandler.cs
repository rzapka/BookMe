using AutoMapper;
using BookMe.Application.Employee.Commands.UpdateEmployee;
using BookMe.Application.Employee.Dto;
using BookMe.Application.Employee.Queries.GetEmployeeById;
using BookMe.Domain.Interfaces;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

public class GetEmployeeByIdQueryHandler : IRequestHandler<GetEmployeeByIdQuery, UpdateEmployeeCommand>
{
    private readonly IEmployeeRepository _employeeRepository;
    private readonly IMapper _mapper;

    public GetEmployeeByIdQueryHandler(IEmployeeRepository employeeRepository, IMapper mapper)
    {
        _employeeRepository = employeeRepository;
        _mapper = mapper;
    }

    public async Task<UpdateEmployeeCommand> Handle(GetEmployeeByIdQuery request, CancellationToken cancellationToken)
    {
        var employee = await _employeeRepository.GetEmployeeByIdAsync(request.Id);
        return employee == null ? null : _mapper.Map<UpdateEmployeeCommand>(employee);
    }
}