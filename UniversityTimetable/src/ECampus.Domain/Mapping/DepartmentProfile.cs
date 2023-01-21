using AutoMapper;
using ECampus.Domain.Mapping.Converters;
using ECampus.Shared.DataTransferObjects;
using ECampus.Shared.Models;

namespace ECampus.Domain.Mapping;

public class DepartmentProfile : Profile
{
    public DepartmentProfile()
    {
        CreateMap<Department, DepartmentDto>().ReverseMap();
        this.CreateListWithPaginationDataMap<Department, DepartmentDto>();
    }
}