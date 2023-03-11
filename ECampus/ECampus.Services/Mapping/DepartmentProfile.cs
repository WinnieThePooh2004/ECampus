using AutoMapper;
using ECampus.Domain.DataTransferObjects;
using ECampus.Domain.Entities;

namespace ECampus.Services.Mapping;

public class DepartmentProfile : Profile
{
    public DepartmentProfile()
    {
        CreateMap<Department, DepartmentDto>().ReverseMap();
    }
}