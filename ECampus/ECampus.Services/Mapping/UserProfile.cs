using AutoMapper;
using ECampus.Domain.DataTransferObjects;
using ECampus.Domain.Entities;
using ECampus.Domain.Responses.User;

namespace ECampus.Services.Mapping;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<UserDto, User>().ForMember(
            dest => dest.Teacher,
            opt =>
                opt.MapFrom(c => new Teacher { Id = c.Teacher!.Id })
        ).ForMember(dest => dest.Student,
            opt =>
                opt.MapFrom(c => new Student { Id = c.Student!.Id }));
        CreateMap<User, UserDto>();
        CreateMap<RegistrationDto, UserDto>();
        CreateMap<Domain.DataTransferObjects.UserProfile, User>().ReverseMap();
        CreateMap<User, MultipleUserResponse>();
    }
}