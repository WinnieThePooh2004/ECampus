using AutoMapper;
using ECampus.Domain.DataTransferObjects;
using ECampus.Domain.Entities;

namespace ECampus.Services.Mapping;

public class FacultyProfile : Profile
{
    public FacultyProfile()
    {
        CreateMap<Faculty, FacultyDto>().ReverseMap();
    }
}