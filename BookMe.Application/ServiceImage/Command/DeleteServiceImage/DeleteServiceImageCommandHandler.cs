using BookMe.Domain.Interfaces;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace BookMe.Application.ServiceImage.Commands.DeleteServiceImage
{
    public class DeleteServiceImageCommandHandler : IRequestHandler<DeleteServiceImageCommand>
    {
        private readonly IServiceImageRepository _serviceImageRepository;

        public DeleteServiceImageCommandHandler(IServiceImageRepository serviceImageRepository)
        {
            _serviceImageRepository = serviceImageRepository;
        }

        public async Task<Unit> Handle(DeleteServiceImageCommand request, CancellationToken cancellationToken)
        {
            await _serviceImageRepository.DeleteServiceImageAsync(request.Id);
            return Unit.Value;
        }
    }
}
