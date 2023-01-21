using AutoMapper;
using ECampus.Shared.DataTransferObjects;
using ECampus.Shared.Models;

namespace ECampus.Domain.Mapping;

public class TaskSubmissionProfile : Profile
{
    public TaskSubmissionProfile()
    {
        CreateMap<TaskSubmission, TaskSubmissionDto>().ReverseMap();
    }
}