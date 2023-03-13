using AutoMapper;
using ECampus.Domain.DataTransferObjects;
using ECampus.Domain.Entities;
using ECampus.Domain.Responses.TaskSubmission;

namespace ECampus.Services.Mapping;

public class TaskSubmissionProfile : Profile
{
    public TaskSubmissionProfile()
    {
        CreateMap<TaskSubmission, TaskSubmissionDto>().ReverseMap();
        CreateMap<TaskSubmission, MultipleTaskSubmissionResponse>().ForMember(
            dest => dest.MaxPoints,
            opt =>
                opt.MapFrom(c => c.CourseTask!.MaxPoints)
        ).ForMember(
            dest => dest.StudentEmail,
            opt =>
                opt.MapFrom(c => c.Student != null ? c.Student.UserEmail : string.Empty)
        ).ForMember(
            dest => dest.StudentEmail,
            opt =>
                opt.MapFrom(c => $"{c.Student!.FirstName} {c.Student.LastName}"));
    }
}