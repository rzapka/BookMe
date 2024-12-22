using AutoMapper;
using BookMe.Application.ServiceCategory.Dto;
using BookMe.Domain.Entities;
using BookMe.Domain.Interfaces;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BookMe.Application.ServiceCategory.Queries.GetAllServiceCategories
{
    public class GetAllServiceCategoriesQueryHandler : IRequestHandler<GetAllServiceCategoriesQuery, IEnumerable<ServiceCategoryDto>>
    {
        private readonly IServiceCategoryRepository _serviceCategoryRepository;
        private readonly IMapper _mapper;

        public GetAllServiceCategoriesQueryHandler(IServiceCategoryRepository serviceCategoryRepository, IMapper mapper)
        {
            _serviceCategoryRepository = serviceCategoryRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ServiceCategoryDto>> Handle(GetAllServiceCategoriesQuery request, CancellationToken cancellationToken)
        {
            var serviceCategories = string.IsNullOrWhiteSpace(request.SearchTerm)
                ? await _serviceCategoryRepository.GetAll()
                : await _serviceCategoryRepository.SearchAsync(request.SearchTerm);
           
            var dtos = _mapper.Map<IEnumerable<ServiceCategoryDto>>(serviceCategories);

            return dtos;
        }
    }
}
