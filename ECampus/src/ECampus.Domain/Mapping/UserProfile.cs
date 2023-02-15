using AutoMapper;
using ECampus.Shared.DataTransferObjects;
using ECampus.Shared.Models;

namespace ECampus.Domain.Mapping;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<UserDto, User>().ReverseMap();
        CreateMap<RegistrationDto, UserDto>();
        CreateMap<Shared.DataTransferObjects.UserProfile, User>().ReverseMap();
    }
}