using AutoMapper;
using BookMe.Application.ApplicationUser.Commands.UpdateApplicationUser;
using BookMe.Application.ApplicationUser.Dto;

namespace BookMe.Application.Mappings
{
    public class ApplicationUserMappingProfile : Profile
    {
        public ApplicationUserMappingProfile()
        {
            CreateMap<Domain.Entities.ApplicationUser, ApplicationUserDto>().ReverseMap();
            CreateMap<Domain.Entities.ApplicationUser, UpdateApplicationUserCommand>().ReverseMap();
        }
    }
}
