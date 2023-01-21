using AutoMapper;
using ECampus.Domain.Mapping.Converters;
using ECampus.Shared.DataTransferObjects;
using ECampus.Shared.Models;

namespace ECampus.Domain.Mapping;

public class GroupProfile : Profile
{
    public GroupProfile() 
    {
        CreateMap<Group, GroupDto>().ReverseMap();
        this.CreateListWithPaginationDataMap<Group, GroupDto>();
    }
}