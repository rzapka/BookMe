using MediatR;

namespace BookMe.Application.ServiceCategory.Commands.DeleteServiceCategory
{
    public class DeleteServiceCategoryCommand : IRequest<Unit>
    {
        public int Id { get; set; }
    }
}
