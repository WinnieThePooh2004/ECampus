using ECampus.Shared.DataTransferObjects;

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
        var result = _baseSelector.GetAllPropertiesNames();
        result.Add(("Max points", "CourseTaskDto.MaxPoints"));
        result.Add(("Student email", "Student.UserEmail"));
        result.Add(("Student Name", "Student.LastName"));
        return result;
    }

    public List<(string displayName, string value)> GetAllProperties(TaskSubmissionDto item)
    {
        var result = _baseSelector.GetAllProperties(item);
        result.Add(("Max points", item.CourseTask!.MaxPoints.ToString()));
        var studentEmail = item.Student.UserEmail;
        result.Add(studentEmail is not null ? ("Email", studentEmail) : ("Email", "-"));
        result.Add(("Student Name", $"{item.Student.LastName} {item.Student.FirstName}"));
        return result;
    }

    public List<string> GetAllPropertiesValues(TaskSubmissionDto item)
    {
        var result = _baseSelector.GetAllPropertiesValues(item);
        result.Add(item.CourseTask!.MaxPoints.ToString());
        var studentEmail = item.Student.UserEmail;
        result.Add(studentEmail ?? "-");
        result.Add( $"{item.Student.LastName} {item.Student.FirstName}");
        return result;
    }
}