using AutoMapper;
using BookMe.Application.ServiceCategory.Dto;
using BookMe.Domain.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookMe.Application.ServiceCategory.Queries.GetServiceCategoryByEncodedName
{
    public class GetServiceCategoryByEncodedNameQueryHandler : IRequestHandler<GetServiceCategoryByEncodedNameQuery, ServiceCategoryDto>
    {
        private readonly IServiceCategoryRepository _serviceCategoryRepository;
        private readonly IMapper _mapper;

        public GetServiceCategoryByEncodedNameQueryHandler(IServiceCategoryRepository serviceCategoryRepository,
            IMapper mapper)
        {
            _serviceCategoryRepository = serviceCategoryRepository;
            _mapper = mapper;
        }
        public async Task<ServiceCategoryDto> Handle(GetServiceCategoryByEncodedNameQuery request, CancellationToken cancellationToken)
        {
            var serviceCategory = await _serviceCategoryRepository.GetByEncodedName(request.EncodedName, request.SearchTerm);
          
            return _mapper.Map<ServiceCategoryDto>(serviceCategory);
        }
    }
}
