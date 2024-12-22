using AutoMapper;
using BookMe.Application.ServiceCategory.Dto;
using BookMe.Application.Service.Dto;
using BookMe.Domain.Entities;
using BookMe.Application.ServiceCategory.Commands.UpdateServiceCategory;

namespace BookMe.Application.Mappings
{
    public class ServiceCategoryMappingProfile : Profile
    {
        public ServiceCategoryMappingProfile()
        {
            CreateMap<Domain.Entities.ServiceCategory, ServiceCategoryDto>();

            CreateMap<Domain.Entities.ServiceCategory, UpdateServiceCategoryCommand>();

        }
    }
}
