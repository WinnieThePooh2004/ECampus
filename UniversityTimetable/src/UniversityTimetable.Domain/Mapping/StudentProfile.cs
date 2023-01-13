using AutoMapper;
using UniversityTimetable.Domain.Mapping.Converters;
using UniversityTimetable.Shared.DataTransferObjects;
using UniversityTimetable.Shared.Models;

namespace UniversityTimetable.Domain.Mapping;

public class StudentProfile : Profile
{
    public StudentProfile()
    {
        CreateMap<Student, StudentDto>().ReverseMap();
        this.CreateListWithPaginationDataMap<Student, StudentDto>();
    }
}