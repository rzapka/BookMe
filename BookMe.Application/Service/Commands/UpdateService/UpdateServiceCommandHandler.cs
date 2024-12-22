using AutoMapper;
using BookMe.Domain.Interfaces;
using BookMe.Application.Exceptions;
using FluentValidation;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using System;
using BookMe.Application.Service.Commands.UpdateService;

namespace BookMe.Application.Service.Commands.UpdateService
{
    public class UpdateServiceCommandHandler : IRequestHandler<UpdateServiceCommand>
    {
        private readonly IServiceRepository _serviceRepository;

        public UpdateServiceCommandHandler(IServiceRepository serviceRepository)
        {
            _serviceRepository = serviceRepository;
        }

        public async Task<Unit> Handle(UpdateServiceCommand request, CancellationToken cancellationToken)
        {
            // Pobieranie aktualizowanego serwisu
            var service = await _serviceRepository.GetServiceByEncodedName(request.EncodedName)
                ?? throw new KeyNotFoundException($"Service with EncodedName {request.EncodedName} not found.");

            // Sprawdzenie, czy istnieje serwis z taką samą nazwą (pomijając aktualny serwis)
            var existingService = await _serviceRepository.GetByNameAsync(request.Name);
            if (existingService != null && existingService.Id != service.Id)
            {
                throw new ServiceNameConflictException("Nazwa serwisu jest już zajęta.");
            }

            // Aktualizacja serwisu
            service.Name = request.Name;
            service.Description = request.Description;
            service.EncodeName();
            service.ImageUrl = request.ImageUrl;
            service.ServiceCategoryId = request.ServiceCategoryId;
            service.ContactDetails.City = request.City;
            service.ContactDetails.Street = request.Street;
            service.ContactDetails.PhoneNumber = request.PhoneNumber;
            service.ContactDetails.PostalCode = request.PostalCode;

            await _serviceRepository.Commit();

            return Unit.Value;
        }
    }
}
