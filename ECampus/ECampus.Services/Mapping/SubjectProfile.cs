using AutoMapper;
using ECampus.Domain.DataTransferObjects;
using ECampus.Domain.Models;

namespace ECampus.Services.Mapping;

public class SubjectProfile : Profile
{
    public SubjectProfile()
    {
        CreateMap<Subject, SubjectDto>().ReverseMap();
    }
}