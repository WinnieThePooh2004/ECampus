using AutoMapper;
using UniversityTimetable.Shared.DataTransferObjects;
using UniversityTimetable.Shared.Extensions;
using UniversityTimetable.Shared.Models;

namespace UniversityTimetable.Domain.Mapping
{
    public class ClassProfile : Profile
    {
        public ClassProfile()
        {
            CreateMap<Class, ClassDto>();
            CreateMap<ClassDto, Class>()
                .IgnoreMember(dest => dest.Group)
                .IgnoreMember(dest => dest.Auditory)
                .IgnoreMember(dest => dest.Teacher)
                .IgnoreMember(dest => dest.Subject);
        }
    }
}
