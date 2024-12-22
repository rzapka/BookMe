using AutoMapper;
using BookMe.Application.ApplicationUser.Dto;
using BookMe.Domain.Interfaces;
using MediatR;

namespace BookMe.Application.ApplicationUser.Queries.GetApplicationUserById
{
    public class GetApplicationUserByIdQueryHandler : IRequestHandler<GetApplicationUserByIdQuery, Domain.Entities.ApplicationUser>
    {
        private readonly IApplicationUserRepository _userRepository;
        public GetApplicationUserByIdQueryHandler(IApplicationUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<Domain.Entities.ApplicationUser> Handle(GetApplicationUserByIdQuery request, CancellationToken cancellationToken)
        {
            return await _userRepository.GetByIdAsync(request.Id);
        }
    }
}
