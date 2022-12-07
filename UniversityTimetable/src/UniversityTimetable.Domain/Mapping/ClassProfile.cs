using AutoMapper;
using UniversityTimetable.Shared.DataTransferObjects;
using UniversityTimetable.Shared.Models;

namespace UniversityTimetable.Domain.Mapping
{
    public class ClassProfile : Profile
    {
        public ClassProfile()
        {
            CreateMap<Class, ClassDTO>().ReverseMap();
            this.CreateListWithPaginationDataMap<Class, ClassDTO>();
        }
    }
}
