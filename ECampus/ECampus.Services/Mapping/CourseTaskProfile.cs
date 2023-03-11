using AutoMapper;
using ECampus.Shared.DataTransferObjects;
using ECampus.Shared.Models;

namespace ECampus.Services.Mapping;

public class CourseTaskProfile : Profile
{
    public CourseTaskProfile()
    {
        CreateMap<CourseTask, CourseTaskDto>().ReverseMap();
    }
}