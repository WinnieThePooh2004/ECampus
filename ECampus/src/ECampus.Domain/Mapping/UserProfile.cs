using AutoMapper;
using ECampus.Shared.DataTransferObjects;
using ECampus.Shared.Models;

namespace ECampus.Domain.Mapping
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<User, UserDto>().ForMember(
                dest => dest.PasswordConfirm,
                opt => opt.MapFrom(c => c.Password)
                );

            CreateMap<UserDto, User>();
        }
    }
}
