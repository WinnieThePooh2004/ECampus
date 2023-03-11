using AutoMapper;
using ECampus.Domain.DataTransferObjects;
using ECampus.Domain.Models;

namespace ECampus.Services.Mapping;

public class CourseTaskProfile : Profile
{
    public CourseTaskProfile()
    {
        CreateMap<CourseTask, CourseTaskDto>().ReverseMap();
    }
}