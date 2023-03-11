using AutoMapper;
using ECampus.Domain.DataTransferObjects;
using ECampus.Domain.Entities;

namespace ECampus.Services.Mapping;

public class AuditoryProfile : Profile
{
    public AuditoryProfile()
    {
        CreateMap<Auditory, AuditoryDto>().ReverseMap();
    }
}