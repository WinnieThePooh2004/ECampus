using AutoMapper;
using ECampus.Shared.DataTransferObjects;
using ECampus.Shared.Models;

namespace ECampus.Domain.Mapping;

public class LogProfile : Profile
{
    public LogProfile()
    {
        CreateMap<Log, LogDto>();
    }
}