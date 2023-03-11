using AutoMapper;
using ECampus.Domain.DataTransferObjects;
using ECampus.Domain.Entities;

namespace ECampus.Services.Mapping;

public class TaskSubmissionProfile : Profile
{
    public TaskSubmissionProfile()
    {
        CreateMap<TaskSubmission, TaskSubmissionDto>().ReverseMap();
    }
}