using AutoMapper;
using BookMe.Application.ServiceCategory.Commands.UpdateServiceCategory;
using BookMe.Application.ServiceCategory.Dto;
using BookMe.Application.ServiceCategory.Queries.GetServiceCategoryById;
using BookMe.Domain.Interfaces;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

public class GetServiceCategoryByIdQueryHandler : IRequestHandler<GetServiceCategoryByIdQuery, UpdateServiceCategoryCommand>
{
    private readonly IServiceCategoryRepository _serviceCategoryRepository;
    private readonly IMapper _mapper;

    public GetServiceCategoryByIdQueryHandler(IServiceCategoryRepository serviceCategoryRepository, IMapper mapper)
    {
        _serviceCategoryRepository = serviceCategoryRepository;
        _mapper = mapper;
    }

    public async Task<UpdateServiceCategoryCommand> Handle(GetServiceCategoryByIdQuery request, CancellationToken cancellationToken)
    {
        var category = await _serviceCategoryRepository.GetByIdAsync(request.Id);
        return _mapper.Map<UpdateServiceCategoryCommand>(category);
    }
}
