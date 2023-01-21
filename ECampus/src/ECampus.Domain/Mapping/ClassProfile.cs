using AutoMapper;
using ECampus.Shared.DataTransferObjects;
using ECampus.Shared.Extensions;
using ECampus.Shared.Models;

namespace ECampus.Domain.Mapping
{
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
}
