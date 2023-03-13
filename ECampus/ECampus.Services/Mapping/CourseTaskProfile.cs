using AutoMapper;
using ECampus.Domain.DataTransferObjects;
using ECampus.Domain.Entities;
using ECampus.Domain.Responses.CourseTask;

namespace ECampus.Services.Mapping;

public class CourseTaskProfile : Profile
{
    public CourseTaskProfile()
    {
        CreateMap<CourseTask, CourseTaskDto>().ReverseMap();
        CreateMap<CourseTask, MultipleCourseTaskResponse>();
    }
}