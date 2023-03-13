using AutoMapper;
using ECampus.Domain.DataTransferObjects;
using ECampus.Domain.Entities;
using ECampus.Domain.Responses.Teacher;

namespace ECampus.Services.Mapping;

public class TeacherProfile : Profile
{
    public TeacherProfile()
    {
        CreateMap<Teacher, TeacherDto>();
        CreateMap<TeacherDto, Teacher>().ForMember(
            dest => dest.Subjects, opt =>
                opt.MapFrom(c => c.Subjects!.Select(subject => 
                    new Subject { Id = subject.Id }).ToList()));
        CreateMap<Teacher, MultipleTeacherResponse>();
    }
}