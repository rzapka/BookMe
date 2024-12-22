using BookMe.Application.ApplicationUser.Dto;
using BookMe.Domain.Entities;
using MediatR;

namespace BookMe.Application.ApplicationUser.Queries.GetApplicationUserById
{
    public class GetApplicationUserByIdQuery : IRequest<Domain.Entities.ApplicationUser>
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
    }
}
