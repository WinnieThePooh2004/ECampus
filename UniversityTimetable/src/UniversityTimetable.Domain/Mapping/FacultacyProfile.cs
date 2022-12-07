using AutoMapper;
using UniversityTimetable.Shared.DataTransferObjects;
using UniversityTimetable.Shared.Models;

namespace UniversityTimetable.Domain.Mapping
{
    public class FacultacyProfile : Profile
    {
        public FacultacyProfile()
        {
            CreateMap<Facultacy, FacultacyDTO>().ReverseMap();
            this.CreateListWithPaginationDataMap<Facultacy, FacultacyDTO>();
        }
    }
}
