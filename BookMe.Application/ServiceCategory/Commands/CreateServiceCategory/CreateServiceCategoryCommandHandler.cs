using BookMe.Domain.Entities;
using BookMe.Domain.Interfaces;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace BookMe.Application.ServiceCategory.Commands.CreateServiceCategory
{
    public class CreateServiceCategoryCommandHandler : IRequestHandler<CreateServiceCategoryCommand>
    {
        private readonly IServiceCategoryRepository _serviceCategoryRepository;

        public CreateServiceCategoryCommandHandler(IServiceCategoryRepository serviceCategoryRepository)
        {
            _serviceCategoryRepository = serviceCategoryRepository;
        }

        public async Task<Unit> Handle(CreateServiceCategoryCommand request, CancellationToken cancellationToken)
        {
            var category = new Domain.Entities.ServiceCategory
            {
                Name = request.Name,
                ImageUrl = request.ImageUrl
            };
            category.EncodeName();

            await _serviceCategoryRepository.AddAsync(category);

            return Unit.Value;
        }
    }
}
