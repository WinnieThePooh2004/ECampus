using AutoMapper;
using ECampus.Domain.Entities;
using ECampus.Domain.Responses.Log;

namespace ECampus.Services.Mapping;

public class LogProfile : Profile
{
    public LogProfile()
    {
        CreateMap<Log, MultipleLogResponse>();
    }
}