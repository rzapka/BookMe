using AutoMapper;
using BookMe.Application.Service.Dto;
using BookMe.Domain.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookMe.Application.Opinion.Queries.GetServiceByOpinionId
{
    public class GetServiceByOpinionIdQueryHandler : IRequestHandler<GetServiceByOpinionIdQuery, ServiceDto>
    {
        private readonly IOpinionRepository _opinionRepository;
        private readonly IMapper _mapper;
        public GetServiceByOpinionIdQueryHandler(IOpinionRepository opinionRepository, IMapper mapper)
        {
            _opinionRepository = opinionRepository;
            _mapper = mapper;
        }
        public async Task<ServiceDto> Handle(GetServiceByOpinionIdQuery request, CancellationToken cancellationToken)
        {
 
            var service = await _opinionRepository.GetServiceByOpinionId(request.Id);

            if (service == null)
            {
                throw new InvalidOperationException("Service not found for the given Opinion Id");
            }

            return _mapper.Map<ServiceDto>(service);
        }
    }
}
