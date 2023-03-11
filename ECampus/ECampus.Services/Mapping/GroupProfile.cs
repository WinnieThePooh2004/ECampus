using AutoMapper;
using ECampus.Domain.DataTransferObjects;
using ECampus.Domain.Entities;

namespace ECampus.Services.Mapping;

public class GroupProfile : Profile
{
    public GroupProfile() 
    {
        CreateMap<Group, GroupDto>().ReverseMap();
    }
}