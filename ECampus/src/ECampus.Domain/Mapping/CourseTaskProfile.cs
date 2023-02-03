using AutoMapper;
using ECampus.Shared.DataTransferObjects;
using ECampus.Shared.Models;

namespace ECampus.Domain.Mapping;

public class CourseTaskProfile : Profile
{
    public CourseTaskProfile()
    {
        CreateMap<CourseTask, CourseTaskDto>().ReverseMap();
    }
}