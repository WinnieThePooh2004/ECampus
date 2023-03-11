using AutoMapper;
using ECampus.Domain.DataTransferObjects;
using ECampus.Domain.Entities;

namespace ECampus.Services.Mapping;

public class LogProfile : Profile
{
    public LogProfile()
    {
        CreateMap<Log, LogDto>();
    }
}