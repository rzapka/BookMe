using BookMe.Domain.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookMe.Application.ApplicationUser.Commands.DeleteApplicationUser
{
    public class DeleteApplicationUserCommandHandler : IRequestHandler<DeleteApplicationUserCommand>
    {
        private readonly IApplicationUserRepository _applicationUserRepository;

        public DeleteApplicationUserCommandHandler(IApplicationUserRepository applicationUserRepository)
        {
            _applicationUserRepository = applicationUserRepository;
        }
        public async Task<Unit> Handle(DeleteApplicationUserCommand request, CancellationToken cancellationToken)
        {
            await _applicationUserRepository.DeleteAsync(request.Id);
            return Unit.Value;
        }
    }
}
