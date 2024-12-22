using BookMe.Domain.Interfaces;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace BookMe.Application.ServiceCategory.Commands.DeleteServiceCategory
{
    public class DeleteServiceCategoryCommandHandler : IRequestHandler<DeleteServiceCategoryCommand>
    {
        private readonly IServiceCategoryRepository _serviceCategoryRepository;

        public DeleteServiceCategoryCommandHandler(
            IServiceCategoryRepository serviceCategoryRepository)
        {
            _serviceCategoryRepository = serviceCategoryRepository;

        }

        public async Task<Unit> Handle(DeleteServiceCategoryCommand request, CancellationToken cancellationToken)
        {
            var category = await _serviceCategoryRepository.GetByIdAsync(request.Id) 
                ?? throw new KeyNotFoundException("ServiceCategory not found.");
            
            await _serviceCategoryRepository.DeleteAsync(category);

            return Unit.Value;
        }
    }
}
