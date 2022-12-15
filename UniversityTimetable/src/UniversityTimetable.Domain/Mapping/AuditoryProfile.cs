using AutoMapper;
using UniversityTimetable.Domain.Mapping.Converters;
using UniversityTimetable.Shared.DataTransferObjects;
using UniversityTimetable.Shared.Models;

namespace UniversityTimetable.Domain.Mapping
{
    public class AuditoryProfile : Profile
    {
        public AuditoryProfile()
        {
            CreateMap<Auditory, AuditoryDTO>().ReverseMap();
            this.CreateListWithPaginationDataMap<Auditory, AuditoryDTO>();
        }
    }
}
