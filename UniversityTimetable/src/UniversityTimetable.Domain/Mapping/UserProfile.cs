using AutoMapper;
using UniversityTimetable.Shared.DataTransferObjects;
using UniversityTimetable.Shared.Models;

namespace UniversityTimetable.Domain.Mapping
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
