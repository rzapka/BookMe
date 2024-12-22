using BookMe.Application.ApplicationUser.Dto;
using BookMe.Domain.Entities;
using MediatR;
using System.Collections.Generic;

namespace BookMe.Application.ApplicationUser.Queries.GetApplicationUsers
{
    public class GetApplicationUsersQuery : IRequest<IEnumerable<ApplicationUserDto>>
    {
        public string SearchTerm { get; set; }

        public GetApplicationUsersQuery(string searchTerm)
        {
            SearchTerm = searchTerm;
        }
    }
}
