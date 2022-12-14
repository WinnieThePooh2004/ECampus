using AutoMapper;
using UniversityTimetable.Domain.Mapping.Converters;
using UniversityTimetable.Shared.DataContainers;
using UniversityTimetable.Shared.DataTransferObjects;
using UniversityTimetable.Shared.Extentions;
using UniversityTimetable.Shared.Models;

namespace UniversityTimetable.Domain.Mapping
{
    public class ClassProfile : Profile
    {
        public ClassProfile()
        {
            CreateMap<Class, ClassDTO>();
            CreateMap<ClassDTO, Class>()
                .IgnoreProperties(dest => dest.Group, dest => dest.Auditory, dest => dest.Teacher, dest => dest.Subject);

            CreateMap<TimetableData, Timetable>().ConvertUsing<TimetableConvert>();
        }
    }
}
