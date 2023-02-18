using AutoMapper;
using ECampus.Shared.DataTransferObjects;
using ECampus.Shared.Models;

namespace ECampus.Domain.Mapping;

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
        CreateMap<Shared.DataTransferObjects.UserProfile, User>().ReverseMap();
    }
}