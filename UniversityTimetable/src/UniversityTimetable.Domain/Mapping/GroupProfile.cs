using AutoMapper;
using UniversityTimetable.Shared.Models;

namespace UniversityTimetable.Domain.Mapping
{
    public class GroupProfile : Profile
    {
        public GroupProfile() 
        {
            CreateMap<Group, GroupProfile>().ReverseMap();
            this.CreateListWithPaginationDataMap<Group, GroupProfile>();
        }
    }
}
