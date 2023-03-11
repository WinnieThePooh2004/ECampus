using AutoMapper;
using ECampus.Domain.DataTransferObjects;
using ECampus.Domain.Entities;

namespace ECampus.Services.Mapping;

public class StudentProfile : Profile
{
    public StudentProfile()
    {
        CreateMap<Student, StudentDto>().ReverseMap();
    }
}