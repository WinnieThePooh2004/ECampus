using AutoMapper;
using ECampus.Domain.DataTransferObjects;
using ECampus.Domain.Entities;

namespace ECampus.Services.Mapping;

public class TeacherProfile : Profile
{
    public TeacherProfile()
    {
        CreateMap<Teacher, TeacherDto>().ReverseMap();
    }
}