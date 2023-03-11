using AutoMapper;
using ECampus.Shared.DataTransferObjects;
using ECampus.Shared.Models;

namespace ECampus.Services.Mapping;

public class LogProfile : Profile
{
    public LogProfile()
    {
        CreateMap<Log, LogDto>();
    }
}