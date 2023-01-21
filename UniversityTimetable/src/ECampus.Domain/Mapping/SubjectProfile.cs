using AutoMapper;
using ECampus.Domain.Mapping.Converters;
using ECampus.Shared.DataTransferObjects;
using ECampus.Shared.Models;

namespace ECampus.Domain.Mapping
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
