using MediatR;
using BookMe.Domain.Interfaces;
using BookMe.Application.Service.Dto;
using AutoMapper;

namespace BookMe.Application.Service.Queries.GetRecommendedServices
{
    public class GetRecommendedServicesQueryHandler : IRequestHandler<GetRecommendedServicesQuery, List<ServiceDto>>
    {
        private readonly IServiceRepository _serviceRepository;
        private readonly IMapper _mapper;

        public GetRecommendedServicesQueryHandler(IServiceRepository serviceRepository, IMapper mapper)
        {
            _serviceRepository = serviceRepository;
            _mapper = mapper;
        }

        public async Task<List<ServiceDto>> Handle(GetRecommendedServicesQuery request, CancellationToken cancellationToken)
        {
            var services = await _serviceRepository.GetRecommendedServicesAsync();
            var dtos = _mapper.Map<List<ServiceDto>>(services);
            return dtos;

        }
    }
}
