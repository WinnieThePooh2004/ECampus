using AutoMapper;
using ECampus.Domain.DataTransferObjects;
using ECampus.Domain.Entities;
using ECampus.Domain.Responses.Group;

namespace ECampus.Services.Mapping;

public class GroupProfile : Profile
{
    public GroupProfile() 
    {
        CreateMap<Group, GroupDto>().ReverseMap();
        CreateMap<Group, MultipleGroupResponse>();
    }
}