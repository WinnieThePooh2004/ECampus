using AutoMapper;
using ECampus.Domain.DataTransferObjects;
using ECampus.Domain.Models;

namespace ECampus.Services.Mapping;

public class AuditoryProfile : Profile
{
    public AuditoryProfile()
    {
        CreateMap<Auditory, AuditoryDto>().ReverseMap();
    }
}