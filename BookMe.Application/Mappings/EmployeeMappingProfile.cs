using AutoMapper;
using BookMe.Application.Employee.Commands.UpdateEmployee;
using BookMe.Application.Employee.Dto;
using BookMe.Domain.Entities;

namespace BookMe.Application.Mappings
{
    public class EmployeeMappingProfile : Profile
    {
        public EmployeeMappingProfile()
        {
            CreateMap<Domain.Entities.Employee, EmployeeDto>()
              .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.User.FirstName))
              .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.User.LastName))
              .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.User.Email))
              .ForMember(dest => dest.AvatarUrl, opt => opt.MapFrom(src => src.User.AvatarUrl))
              .ForMember(dest => dest.ServiceId, opt => opt.MapFrom(src => src.ServiceId))
              .ForMember(dest => dest.ServiceName, opt => opt.MapFrom(src => src.Service.Name))
              .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.User.FirstName + " " + src.User.LastName)) 
              .ReverseMap();

            CreateMap<Domain.Entities.Employee, UpdateEmployeeCommand>()
              .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.User.FirstName))
              .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.User.LastName))
              .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.User.Email))
              .ForMember(dest => dest.AvatarUrl, opt => opt.MapFrom(src => src.User.AvatarUrl))
              .ForMember(dest => dest.ServiceId, opt => opt.MapFrom(src => src.ServiceId))
              .ForMember(dest => dest.ServiceName, opt => opt.MapFrom(src => src.Service.Name))
              .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.User.FirstName + " " + src.User.LastName)) 
              .ReverseMap();


            // Mapowanie CreateEmployeeDto na ApplicationUser
            CreateMap<EmployeeDto, Domain.Entities.ApplicationUser>()
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FirstName))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.Gender, opt => opt.MapFrom(src => src.Gender))
                .ForMember(dest => dest.Gender, opt => opt.MapFrom(src => src.Gender))
                .ForMember(dest => dest.AvatarUrl, opt => opt.MapFrom(src => src.AvatarUrl));

            // Mapowanie EmployeeDto na Employee
            CreateMap<EmployeeDto, Domain.Entities.Employee>()
                .ForMember(dest => dest.ServiceId, opt => opt.MapFrom(src => src.ServiceId))
                .ForMember(dest => dest.UserId, opt => opt.Ignore()) // Zostanie ustawione po utworzeniu użytkownika
                .ForMember(dest => dest.User, opt => opt.Ignore()); // Oddzielnie tworzymy użytkownika
        }
    }
}
