using AutoMapper;
using BookMe.Application.Service.Dto;
using BookMe.Domain.Interfaces;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace BookMe.Application.Service.Queries.GetServiceByEncodedName
{
    public class GetServiceByEncodedNameQueryHandler : IRequestHandler<GetServiceByEncodedNameQuery, ServiceDto>
    {
        private readonly IServiceRepository _serviceRepository;
        private readonly IMapper _mapper;

        public GetServiceByEncodedNameQueryHandler(IServiceRepository serviceRepository, IMapper mapper)
        {
            _serviceRepository = serviceRepository;
            _mapper = mapper;
        }

        public async Task<ServiceDto> Handle(GetServiceByEncodedNameQuery request, CancellationToken cancellationToken)
        {
            var service = await _serviceRepository.GetServiceByEncodedName(request.EncodedName);
            return _mapper.Map<ServiceDto>(service);
        }
    }
}
