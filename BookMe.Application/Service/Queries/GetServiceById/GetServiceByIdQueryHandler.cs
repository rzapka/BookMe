using AutoMapper;
using BookMe.Application.Service.Dto;
using BookMe.Domain.Entities;
using BookMe.Domain.Interfaces;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace BookMe.Application.Service.Queries.GetServiceById
{
    public class GetServiceByIdQueryHandler : IRequestHandler<GetServiceByIdQuery, ServiceDto>
    {
        private readonly IServiceRepository _serviceRepository;
        private readonly IMapper _mapper;

        public GetServiceByIdQueryHandler(IServiceRepository serviceRepository, IMapper mapper)
        {
            _serviceRepository = serviceRepository;
            _mapper = mapper;
        }

        public async Task<ServiceDto> Handle(GetServiceByIdQuery request, CancellationToken cancellationToken)
        {
            var service = await _serviceRepository.GetServiceByIdAsync(request.Id);
            var dto = _mapper.Map<ServiceDto>(service);
            return dto;
        }
    }
}
