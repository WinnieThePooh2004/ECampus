using AutoMapper;
using ECampus.Domain.DataTransferObjects;
using ECampus.Domain.Entities;
using ECampus.Domain.Responses.Course;

namespace ECampus.Services.Mapping;

public class CourseProfile : Profile
{
    public CourseProfile()
    {
        CreateMap<Course, CourseDto>();
        
        CreateMap<CourseDto, Course>().ForMember(
            dest => dest.Teachers, opt =>
                opt.MapFrom(c => c.Teachers!.Select(subject => 
                    new Subject { Id = subject.Id }).ToList())).ForMember(
            dest => dest.Groups, opt =>
                opt.MapFrom(c => c.Groups!.Select(subject => 
                    new Subject { Id = subject.Id }).ToList()));
        
        CreateMap<Course, CourseSummaryResponse>().ForMember(
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

        CreateMap<Course, MultipleCourseResponse>();
    }
}