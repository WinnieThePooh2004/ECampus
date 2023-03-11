using AutoMapper;
using ECampus.Shared.DataTransferObjects;
using ECampus.Shared.Models;

namespace ECampus.Services.Mapping;

public class TeacherProfile : Profile
{
    public TeacherProfile()
    {
        CreateMap<Teacher, TeacherDto>().ReverseMap();
    }
}