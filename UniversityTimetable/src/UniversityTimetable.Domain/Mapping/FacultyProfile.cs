using AutoMapper;
using UniversityTimetable.Domain.Mapping.Converters;
using UniversityTimetable.Shared.DataTransferObjects;
using UniversityTimetable.Shared.Models;

namespace UniversityTimetable.Domain.Mapping;

public class FacultyProfile : Profile
{
    public FacultyProfile()
    {
        CreateMap<Faculty, FacultyDto>().ReverseMap();
        this.CreateListWithPaginationDataMap<Faculty, FacultyDto>();
    }
}