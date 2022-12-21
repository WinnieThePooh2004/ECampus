using AutoMapper;
using UniversityTimetable.Domain.Mapping.Converters;
using UniversityTimetable.Shared.DataTransferObjects;
using UniversityTimetable.Shared.Models;

namespace UniversityTimetable.Domain.Mapping
{
    public class SubjectProfile : Profile
    {
        public SubjectProfile()
        {
            CreateMap<Subject, SubjectDto>().ReverseMap();
            this.CreateListWithPaginationDataMap<Subject, SubjectDto>();
        }
    }
}
