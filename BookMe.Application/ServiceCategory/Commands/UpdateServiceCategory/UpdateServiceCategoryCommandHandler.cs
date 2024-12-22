using BookMe.Domain.Entities;
using BookMe.Domain.Interfaces;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace BookMe.Application.ServiceCategory.Commands.UpdateServiceCategory
{
    public class UpdateServiceCategoryCommandHandler : IRequestHandler<UpdateServiceCategoryCommand>
    {
        private readonly IServiceCategoryRepository _serviceCategoryRepository;

        public UpdateServiceCategoryCommandHandler(IServiceCategoryRepository serviceCategoryRepository)
        {
            _serviceCategoryRepository = serviceCategoryRepository;
        }

        public async Task<Unit> Handle(UpdateServiceCategoryCommand request, CancellationToken cancellationToken)
        {
            var category = await _serviceCategoryRepository.GetByIdAsync(request.Id);
            if (category == null)
            {
                throw new KeyNotFoundException("ServiceCategory not found.");
            }

            category.Name = request.Name;
            category.ImageUrl = request.ImageUrl;
            category.EncodeName();

            await _serviceCategoryRepository.UpdateAsync(category);

            return Unit.Value;
        }
    }
}
