using AutoMapper;
using ECampus.Domain.DataTransferObjects;
using ECampus.Domain.Entities;
using ECampus.Domain.Responses.Auditory;

namespace ECampus.Services.Mapping;

public class AuditoryProfile : Profile
{
    public AuditoryProfile()
    {
        CreateMap<Auditory, AuditoryDto>().ReverseMap();
        CreateMap<Auditory, MultipleAuditoryResponse>();
    }
}