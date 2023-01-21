using AutoMapper;
using UniversityTimetable.Shared.DataTransferObjects;
using UniversityTimetable.Shared.Models;

namespace UniversityTimetable.Domain.Mapping;

public class TaskSubmissionProfile : Profile
{
    public TaskSubmissionProfile()
    {
        CreateMap<TaskSubmission, TaskSubmissionDto>().ReverseMap();
    }
}