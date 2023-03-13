using AutoMapper;
using ECampus.Domain.DataTransferObjects;
using ECampus.Domain.Entities;
using ECampus.Domain.Responses.Subject;

namespace ECampus.Services.Mapping;

public class SubjectProfile : Profile
{
    public SubjectProfile()
    {
        CreateMap<Subject, SubjectDto>();
        CreateMap<SubjectDto, Subject>().ForMember(
            dest => dest.Teachers, opt =>
                opt.MapFrom(c => c.Teachers!.Select(subject => 
                    new Subject { Id = subject.Id }).ToList()));
        CreateMap<Subject, MultipleSubjectResponse>();
    }
}