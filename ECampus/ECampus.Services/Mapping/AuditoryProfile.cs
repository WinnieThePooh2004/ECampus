using AutoMapper;
using ECampus.Domain.Commands.Auditory;
using ECampus.Domain.Entities;
using ECampus.Domain.Responses.Auditory;

namespace ECampus.Services.Mapping;

public class AuditoryProfile : Profile
{
    public AuditoryProfile()
    {
        CreateMap<Auditory, MultipleAuditoryResponse>();
        CreateMap<Auditory, SingleAuditoryResponse>();
        
        CreateMap<CreateAuditoryCommand, Auditory>();
        CreateMap<UpdateAuditoryCommand, Auditory>();
    }
}