using AutoMapper;
using ECampus.Domain.DataTransferObjects;
using ECampus.Domain.Extensions;
using ECampus.Domain.Models;

namespace ECampus.Services.Mapping;

public class ClassProfile : Profile
{
    public ClassProfile()
    {
        CreateMap<Class, ClassDto>();
        CreateMap<ClassDto, Class>()
            .IgnoreMember(dest => dest.Group)
            .IgnoreMember(dest => dest.Auditory)
            .IgnoreMember(dest => dest.Teacher)
            .IgnoreMember(dest => dest.Subject);
    }
}