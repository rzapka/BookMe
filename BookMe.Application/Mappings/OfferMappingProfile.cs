using AutoMapper;
using BookMe.Application.Offer.Commands.CreateOffer;
using BookMe.Application.Offer.Commands.UpdateOffer;
using BookMe.Application.Offer.Dto;
using BookMe.Domain.Entities;

namespace BookMe.Application.Mappings
{
    public class OfferMappingProfile : Profile
    {
        public OfferMappingProfile()
        {
            CreateMap<Domain.Entities.Offer, OfferDto>()
               .ForMember(dest => dest.ServiceName, opt => opt.MapFrom(src => src.Service.Name))
               .ForMember(dest => dest.ServiceEncodedName, opt => opt.MapFrom(src => src.Service.EncodedName))
               .ReverseMap();

            CreateMap<Domain.Entities.Offer, CreateOfferCommand>()
              .ForMember(dest => dest.ServiceEncodedName, opt => opt.MapFrom(src => src.Service.EncodedName))
              .ReverseMap();

            CreateMap<Domain.Entities.Offer, UpdateOfferCommand>()
              .ForMember(dest => dest.ServiceEncodedName, opt => opt.MapFrom(src => src.Service.EncodedName))
              .ReverseMap();
        }
    }
}
