using AutoMapper;
using ECampus.Domain.DataTransferObjects;
using ECampus.Domain.Models;

namespace ECampus.Services.Mapping;

public class TeacherProfile : Profile
{
    public TeacherProfile()
    {
        CreateMap<Teacher, TeacherDto>().ReverseMap();
    }
}