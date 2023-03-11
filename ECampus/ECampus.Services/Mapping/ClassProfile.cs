using AutoMapper;
using ECampus.Domain.DataTransferObjects;
using ECampus.Domain.Entities;
using ECampus.Domain.Extensions;

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