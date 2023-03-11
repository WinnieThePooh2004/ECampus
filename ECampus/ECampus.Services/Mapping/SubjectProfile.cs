using AutoMapper;
using ECampus.Domain.DataTransferObjects;
using ECampus.Domain.Entities;

namespace ECampus.Services.Mapping;

public class SubjectProfile : Profile
{
    public SubjectProfile()
    {
        CreateMap<Subject, SubjectDto>().ReverseMap();
    }
}