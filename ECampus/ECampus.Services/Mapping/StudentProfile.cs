using AutoMapper;
using ECampus.Domain.DataTransferObjects;
using ECampus.Domain.Entities;
using ECampus.Domain.Responses.Student;

namespace ECampus.Services.Mapping;

public class StudentProfile : Profile
{
    public StudentProfile()
    {
        CreateMap<Student, StudentDto>().ReverseMap();
        CreateMap<Student, MultipleStudentResponse>();
    }
}