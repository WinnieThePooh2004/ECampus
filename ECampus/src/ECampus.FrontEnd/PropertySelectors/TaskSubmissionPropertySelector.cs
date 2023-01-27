﻿using ECampus.Shared.DataTransferObjects;

namespace ECampus.FrontEnd.PropertySelectors;

public class TaskSubmissionPropertySelector : IPropertySelector<TaskSubmissionDto>
{
    private readonly IPropertySelector<TaskSubmissionDto> _baseSelector;

    public TaskSubmissionPropertySelector(IPropertySelector<TaskSubmissionDto> baseSelector)
    {
        _baseSelector = baseSelector;
    }

    public List<(string displayName, string propertyName)> GetAllPropertiesNames()
    {
        var result = new List<(string displayName, string propertyName)>
        {
            ("Max points", "CourseTaskDto.MaxPoints"),
            ("Student email", "Student.UserEmail"),
            ("Student Name", "Student.LastName")
        };
        result.AddRange(_baseSelector.GetAllPropertiesNames());
        return result;
    }

    public List<(string displayName, string value)> GetAllProperties(TaskSubmissionDto item)
    {
        var studentEmail = item.Student?.UserEmail;
        var result = new List<(string displayName, string value)>
        {
            ("Max points", item.CourseTask?.MaxPoints.ToString() ?? "-"),
            studentEmail is not null ? ("Email", studentEmail) : ("Email", "-"),
            ("Student Name", $"{item.Student?.LastName} {item.Student?.FirstName}")
        };
        result.AddRange(_baseSelector.GetAllProperties(item));
        return result;
    }

    public List<string> GetAllPropertiesValues(TaskSubmissionDto item)
    {
        var studentEmail = item.Student!.UserEmail;
        var result = new List<string>
        {
            item.CourseTask!.MaxPoints.ToString(),
            studentEmail ?? "-",
            $"{item.Student.LastName} {item.Student.FirstName}"
        };
        result.AddRange(_baseSelector.GetAllPropertiesValues(item));
        return result;
    }
}