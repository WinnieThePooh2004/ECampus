using AutoMapper;
using ECampus.Shared.DataTransferObjects;
using ECampus.Shared.Models;

namespace ECampus.Services.Mapping;

public class SubjectProfile : Profile
{
    public SubjectProfile()
    {
        CreateMap<Subject, SubjectDto>().ReverseMap();
    }
}