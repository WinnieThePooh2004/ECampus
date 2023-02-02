using AutoMapper;
using ECampus.Shared.DataTransferObjects;
using ECampus.Shared.Models;

namespace ECampus.Domain.Mapping;

public class CourseProfile : Profile
{
    public CourseProfile()
    {
        CreateMap<Course, CourseDto>().ReverseMap();
        CreateMap<Course, CourseSummary>().ForMember(
            dest => dest.CourseId,
            opt => opt.MapFrom(course => course.Id)
        ).ForMember(
            dest => dest.TeacherNames,
            opt => opt.MapFrom(course =>
                course.Teachers!.Select(teacher => $"{teacher.LastName} {teacher.FirstName}"))
        ).ForMember(
            dest => dest.TotalPoints,
            opt => opt.MapFrom(course => 
                course.Tasks!.Select(task => task.Submissions!.Single().AbsolutePoints()).Sum())
        );
    }
}