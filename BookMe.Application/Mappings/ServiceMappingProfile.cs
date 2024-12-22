using AutoMapper;
using BookMe.Application.Employee.Dto;
using BookMe.Application.Offer.Dto;
using BookMe.Application.OpeningHours.Dto;
using BookMe.Application.Service.Commands.UpdateService;
using BookMe.Application.Service.Dto;
using BookMe.Domain.Entities;

namespace BookMe.Application.Mappings
{
    public class ServiceMappingProfile : Profile
    {
        public ServiceMappingProfile()
        {

            CreateMap<Domain.Entities.Service, ServiceDto>()
                .ForMember(dest => dest.ServiceCategoryName, opt => opt.MapFrom(src => src.ServiceCategory != null ? src.ServiceCategory.Name : null))
                .ForMember(dest => dest.City, opt => opt.MapFrom(src => src.ContactDetails.City))
                .ForMember(dest => dest.Street, opt => opt.MapFrom(src => src.ContactDetails.Street))
                .ForMember(dest => dest.PostalCode, opt => opt.MapFrom(src => src.ContactDetails.PostalCode))
                .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.ContactDetails.PhoneNumber))
                .ForMember(dest => dest.Offers, opt => opt.MapFrom(src => src.Offers))
                .ForMember(dest => dest.Employees, opt => opt.MapFrom(src => src.Employees))
                .ForMember(dest => dest.OpeningHours, opt => opt.MapFrom(src => src.OpeningHours))
                .ReverseMap()
                .ForMember(dest => dest.ContactDetails, opt => opt.MapFrom(src => new ServiceContactDetails
                {
                    City = src.City,
                    Street = src.Street,
                    PostalCode = src.PostalCode,
                    PhoneNumber = src.PhoneNumber
                }))
                .ForMember(dest => dest.ServiceCategory, opt => opt.Ignore())
                .ForMember(dest => dest.EncodedName, opt => opt.Ignore());


            CreateMap<ServiceDto, UpdateServiceCommand>();

        }

    }
}
