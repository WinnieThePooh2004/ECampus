using AutoMapper;
using UniversityTimetable.Domain.Mapping.Converters;
using UniversityTimetable.Shared.DataTransferObjects;
using UniversityTimetable.Shared.Models;

namespace UniversityTimetable.Domain.Mapping;

public class CourseTaskProfile : Profile
{
    public CourseTaskProfile()
    {
        CreateMap<CourseTask, CourseTaskDto>().ReverseMap();
        this.CreateListWithPaginationDataMap<CourseTask, CourseTaskDto>();
    }
}