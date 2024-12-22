using AutoMapper;
using BookMe.Application.Booking.Commands.UpdateBooking;
using BookMe.Application.Booking.Dto;
using BookMe.Domain.Entities;

public class BookingMappingProfile : Profile
{
    public BookingMappingProfile()
    {
        CreateMap<Booking, BookingDto>()
            .ForMember(dest => dest.OfferName, opt => opt.MapFrom(src => src.Offer.Name))
            .ForMember(dest => dest.ServiceName, opt => opt.MapFrom(src => src.Offer.Service.Name))
            .ForMember(dest => dest.ServiceEncodedName, opt => opt.MapFrom(src => src.Offer.Service.EncodedName))
            .ForMember(dest => dest.ServiceId, opt => opt.MapFrom(src => src.Offer.Service.Id))
            .ForMember(dest => dest.OfferPrice, opt => opt.MapFrom(src => src.Offer.Price))
            .ForMember(dest => dest.OfferDuration, opt => opt.MapFrom(src => src.Offer.Duration))
            .ForMember(dest => dest.EmployeeFullName, opt => opt.MapFrom(src => src.Employee != null ? src.Employee.User.FirstName + " " + src.Employee.User.LastName : ""))
            .ForMember(dest => dest.UserFullName, opt => opt.MapFrom(src => src.User != null ? src.User.FirstName + " " + src.User.LastName : ""))
            .ForMember(dest => dest.OpinionId, opt => opt.MapFrom(src => src.Opinion.Id))
            .ForMember(dest => dest.OpinionRating, opt => opt.MapFrom(src => src.Opinion.Rating))
            .ForMember(dest => dest.OpinionContent, opt => opt.MapFrom(src => src.Opinion.Content));

        CreateMap<Booking, UpdateBookingCommand>()
            .ForMember(dest => dest.OfferName, opt => opt.MapFrom(src => src.Offer.Name))
            .ForMember(dest => dest.OfferEncodedName, opt => opt.MapFrom(src => src.Offer.EncodedName))
            .ForMember(dest => dest.ServiceEncodedName, opt => opt.MapFrom(src => src.Service.EncodedName))
            .ForMember(dest => dest.ServiceName, opt => opt.MapFrom(src => src.Offer.Service.Name))
            .ForMember(dest => dest.ServiceId, opt => opt.MapFrom(src => src.Offer.Service.Id))
            .ForMember(dest => dest.OfferPrice, opt => opt.MapFrom(src => src.Offer.Price))
            .ForMember(dest => dest.OfferDuration, opt => opt.MapFrom(src => src.Offer.Duration))
            .ForMember(dest => dest.EmployeeFullName, opt => opt.MapFrom(src => src.Employee != null ? src.Employee.User.FirstName + " " + src.Employee.User.LastName : ""))
            .ForMember(dest => dest.OpinionId, opt => opt.MapFrom(src => src.Opinion.Id))
            .ForMember(dest => dest.OpinionRating, opt => opt.MapFrom(src => src.Opinion.Rating))
            .ForMember(dest => dest.OpinionContent, opt => opt.MapFrom(src => src.Opinion.Content));

        CreateMap<Offer, BookingDto>()
           .ForMember(dest => dest.OfferId, opt => opt.MapFrom(src => src.Id))
           .ForMember(dest => dest.OfferName, opt => opt.MapFrom(src => src.Name))
           .ForMember(dest => dest.ServiceName, opt => opt.MapFrom(src => src.Service.Name))
           .ForMember(dest => dest.ServiceId, opt => opt.MapFrom(src => src.Service.Id))
           .ForMember(dest => dest.OfferPrice, opt => opt.MapFrom(src => src.Price))
           .ForMember(dest => dest.OfferDuration, opt => opt.MapFrom(src => src.Duration));
    }
}
