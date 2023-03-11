using AutoMapper;
using ECampus.Domain.DataTransferObjects;
using ECampus.Domain.Models;

namespace ECampus.Services.Mapping;

public class TeacherRateProfile : Profile
{
    public TeacherRateProfile()
    {
        CreateMap<TeacherRate, TeacherRateDto>().ForMember(
            dest => dest.CourseName,
            opt =>
                opt.MapFrom(c => c.Course!.Name)
        ).ForMember(
            dest => dest.TeachersName,
            opt =>
                opt.MapFrom(c => c.Teacher!.FirstName + c.Teacher.LastName)
        );
    }
}