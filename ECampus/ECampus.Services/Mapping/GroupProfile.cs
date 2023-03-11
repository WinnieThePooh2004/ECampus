using AutoMapper;
using ECampus.Domain.DataTransferObjects;
using ECampus.Domain.Models;

namespace ECampus.Services.Mapping;

public class GroupProfile : Profile
{
    public GroupProfile() 
    {
        CreateMap<Group, GroupDto>().ReverseMap();
    }
}