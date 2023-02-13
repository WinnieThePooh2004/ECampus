using AutoMapper;
using ECampus.Shared.DataTransferObjects;
using ECampus.Shared.Models;

namespace ECampus.Domain.Mapping;

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