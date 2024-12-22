using AutoMapper;
using BookMe.Application.Service.Dto;
using BookMe.Application.Service.Queries.SearchServices;
using BookMe.Domain.Interfaces;
using MediatR;

public class SearchServicesByNameQueryHandler : IRequestHandler<SearchServicesByNameQuery, IEnumerable<ServiceDto>>
{
    private readonly IServiceRepository _serviceRepository;
    private readonly IMapper _mapper;

    public SearchServicesByNameQueryHandler(IServiceRepository serviceRepository, IMapper mapper)
    {
        _serviceRepository = serviceRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<ServiceDto>> Handle(SearchServicesByNameQuery request, CancellationToken cancellationToken)
    {
        var services = await _serviceRepository.SearchServicesAsync(request.SearchTerm);
        return _mapper.Map<IEnumerable<ServiceDto>>(services);
    }
}
