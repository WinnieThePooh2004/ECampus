using AutoMapper;
using ECampus.Shared.DataTransferObjects;
using ECampus.Shared.Models;

namespace ECampus.Services.Mapping;

public class StudentProfile : Profile
{
    public StudentProfile()
    {
        CreateMap<Student, StudentDto>().ReverseMap();
    }
}