using AutoMapper;
using ECampus.Domain.DataTransferObjects;
using ECampus.Domain.Entities;
using ECampus.Domain.Responses.Faculty;

namespace ECampus.Services.Mapping;

public class FacultyProfile : Profile
{
    public FacultyProfile()
    {
        CreateMap<Faculty, FacultyDto>().ReverseMap();
        CreateMap<Faculty, MultipleFacultyResponse>();
    }
}