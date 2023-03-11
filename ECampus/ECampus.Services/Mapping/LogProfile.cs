using AutoMapper;
using ECampus.Domain.DataTransferObjects;
using ECampus.Domain.Models;

namespace ECampus.Services.Mapping;

public class LogProfile : Profile
{
    public LogProfile()
    {
        CreateMap<Log, LogDto>();
    }
}