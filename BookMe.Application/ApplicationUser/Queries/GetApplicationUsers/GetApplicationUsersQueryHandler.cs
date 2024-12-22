using AutoMapper;
using BookMe.Application.ApplicationUser.Dto;
using BookMe.Domain.Entities;
using BookMe.Domain.Interfaces;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace BookMe.Application.ApplicationUser.Queries.GetApplicationUsers
{
    public class GetApplicationUsersQueryHandler : IRequestHandler<GetApplicationUsersQuery, IEnumerable<ApplicationUserDto>>
    {
        private readonly IApplicationUserRepository _userRepository;
        private readonly IMapper _mapper;
        public GetApplicationUsersQueryHandler(IApplicationUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ApplicationUserDto>> Handle(GetApplicationUsersQuery request, CancellationToken cancellationToken)
        {
            IEnumerable<Domain.Entities.ApplicationUser> users;
            if (string.IsNullOrWhiteSpace(request.SearchTerm))
            {
                users = await _userRepository.GetUsersWithoutEmployeesAsync();
            }
            else
            {
                users = await _userRepository.SearchAsync(request.SearchTerm);
            }

            var dtos = _mapper.Map<IEnumerable<ApplicationUserDto>>(users);
            return dtos;
        }
    }
}
