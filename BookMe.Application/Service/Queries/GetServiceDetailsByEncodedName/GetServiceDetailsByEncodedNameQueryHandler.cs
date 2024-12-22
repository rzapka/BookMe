using AutoMapper;
using BookMe.Application.Service.Dto;
using BookMe.Domain.Interfaces;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace BookMe.Application.Service.Queries.GetServiceDetailsByEncodedName
{
    public class GetServiceDetailsByEncodedNameQueryHandler : IRequestHandler<GetServiceDetailsByEncodedNameQuery, ServiceDto>
    {
        private readonly IServiceRepository _serviceRepository;
        private readonly IMapper _mapper;

        public GetServiceDetailsByEncodedNameQueryHandler(IServiceRepository serviceRepository, IMapper mapper)
        {
            _serviceRepository = serviceRepository;
            _mapper = mapper;
        }

        public async Task<ServiceDto> Handle(GetServiceDetailsByEncodedNameQuery request, CancellationToken cancellationToken)
        {
            var service = await _serviceRepository.GetServiceDetailsByEncodedName(request.EncodedName);
            return _mapper.Map<ServiceDto>(service);
        }
    }
}
