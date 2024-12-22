using AutoMapper;
using BookMe.Application.Service.Commands.CreateService;
using BookMe.Domain.Entities;
using BookMe.Domain.Interfaces;
using FluentValidation;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

public class CreateServiceCommandHandler : IRequestHandler<CreateServiceCommand>
{
    private readonly IServiceRepository _serviceRepository;
    private readonly IMapper _mapper;

    public CreateServiceCommandHandler(IServiceRepository serviceRepository, IMapper mapper)
    {
        _serviceRepository = serviceRepository;
        _mapper = mapper;
    }

    public async Task<Unit> Handle(CreateServiceCommand request, CancellationToken cancellationToken)
    {
        // Sprawdzenie, czy nazwa serwisu już istnieje
        var existingService = await _serviceRepository.GetByNameAsync(request.Name);
        if (existingService != null)
        {
            throw new ValidationException("Nazwa serwisu jest już zajęta.");
        }

        var service = _mapper.Map<Service>(request);
        service.EncodeName();
        await _serviceRepository.AddAsync(service);

        return Unit.Value;
    }
}
