using AutoMapper;
using ECampus.Domain.DataTransferObjects;
using ECampus.Domain.Models;

namespace ECampus.Services.Mapping;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<UserDto, User>().ForMember(
            dest => dest.Password,
            // in cases when user is created by admin password always will be 'tempPass1'
            opt => opt.MapFrom(c => "tempPass1")
        );
        CreateMap<User, UserDto>();
        CreateMap<RegistrationDto, UserDto>();
        CreateMap<UserProfile, User>().ReverseMap();
    }
}