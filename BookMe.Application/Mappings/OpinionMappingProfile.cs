using AutoMapper;
using BookMe.Application.Opinion.Commands.CreateOpinion;
using BookMe.Application.Opinion.Commands.UpdateOpinion;
using BookMe.Application.Opinion.Dto;
using BookMe.Domain.Entities;

namespace BookMe.Application.Mappings
{
    public class OpinionMappingProfile : Profile
    {
        public OpinionMappingProfile()
        {
            CreateMap<Domain.Entities.Opinion, OpinionDto>()
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.User.FirstName))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.User.LastName))
                .ForMember(dest => dest.AvatarUrl, opt => opt.MapFrom(src => src.User.AvatarUrl))
                .ForMember(dest => dest.OfferName, opt => opt.MapFrom(src => src.Offer.Name))
                .ForMember(dest => dest.EmployeeFullName, opt => opt.MapFrom(src => $"{src.Employee.User.FirstName} {src.Employee.User.LastName}"));

            CreateMap<Domain.Entities.Opinion, CreateOpinionCommand>()
                .ForMember(dest => dest.ServiceEncodedName, opt => opt.MapFrom(src => src.Service.EncodedName));

            CreateMap<Domain.Entities.Opinion, UpdateOpinionCommand>()
            .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.User.FirstName))
            .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.User.LastName))
            .ForMember(dest => dest.AvatarUrl, opt => opt.MapFrom(src => src.User.AvatarUrl))
            .ForMember(dest => dest.ServiceEncodedName, opt => opt.MapFrom(src => src.Service.EncodedName))
            .ForMember(dest => dest.ServiceName, opt => opt.MapFrom(src => src.Service.Name))
            .ForMember(dest => dest.EmployeeFullName, opt => opt.MapFrom(src => $"{src.Employee.User.FirstName} {src.Employee.User.LastName}"))
            .ForMember(dest => dest.OfferName, opt => opt.MapFrom(src => src.Offer.Name))
            .ForMember(dest => dest.OfferPrice, opt => opt.MapFrom(src => src.Offer.Price))
            .ForMember(dest => dest.OfferDuration, opt => opt.MapFrom(src => src.Offer.Duration))
            .ForMember(dest => dest.BookingStartTime, opt => opt.MapFrom(src => src.Booking.StartTime))
            .ReverseMap(); 
        }
    }
}
