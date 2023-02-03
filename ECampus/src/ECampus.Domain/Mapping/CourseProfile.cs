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
                string.Join(",", course.Teachers!.Select(teacher => $"{teacher.LastName} {teacher.FirstName}")))
        ).ForMember(
            dest => dest.ScoredPoints,
            opt => opt.MapFrom(course =>
                course.Tasks!.Select(task => task.Submissions!.Single().AbsolutePoints()).Sum())
        ).ForMember(
            dest => dest.MaxPoints,
            opt => opt.MapFrom(c =>
                c.Tasks!.Select(task => task.Coefficient * task.MaxPoints).Sum())
        );
    }
}