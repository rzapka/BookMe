using AutoMapper;
using BookMe.Application.Notification.Dto;
using BookMe.Domain.Entities;

namespace BookMe.Application.Mappings
{
    public class NotificationMappingProfile : Profile
    {
        public NotificationMappingProfile()
        {
            CreateMap<Domain.Entities.Notification, NotificationDto>();
        }
    }
}
