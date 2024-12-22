using AutoMapper;
using BookMe.Application.OpeningHours.Commands.CreateOpeningHour;
using BookMe.Application.OpeningHours.Commands.UpdateOpeningHour;
using BookMe.Application.OpeningHours.Dto;
using BookMe.Domain.Entities;

namespace BookMe.Application.Mappings
{
    public class OpeningHourMappingProfile : Profile
    {
        public OpeningHourMappingProfile()
        {
            CreateMap<OpeningHour, OpeningHourDto>()
                .ForMember(dest => dest.ServiceName, opt => opt.MapFrom(src => src.Service.Name))
                .ForMember(dest => dest.ServiceEncodedName, opt => opt.MapFrom(src => src.Service.EncodedName))
                .ReverseMap();

            CreateMap<OpeningHour, UpdateOpeningHourCommand>()
                .ForMember(dest => dest.ServiceName, opt => opt.MapFrom(src => src.Service.Name))
                .ForMember(dest => dest.ServiceEncodedName, opt => opt.MapFrom(src => src.Service.EncodedName))
                .ReverseMap();

            CreateMap<OpeningHourDto, CreateOpeningHourCommand>().ReverseMap();
        }
    }
}
