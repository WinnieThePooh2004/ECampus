using ECampus.FrontEnd.PropertySelectors;
using ECampus.Shared.DataTransferObjects;

namespace ECampus.Tests.Unit.Tests.FrontEnd.PropertySelectors;

public class TaskSubmissionPropertySelectorTests
{
    private readonly TaskSubmissionPropertySelector _sut;

    private readonly IPropertySelector<TaskSubmissionDto> _baseSelector =
        Substitute.For<IPropertySelector<TaskSubmissionDto>>();

    public TaskSubmissionPropertySelectorTests()
    {
        _sut = new TaskSubmissionPropertySelector(_baseSelector);
    }

    [Fact]
    public void GetAllPropertiesNames_ShouldAdd3MoreNames()
    {
        _baseSelector.GetAllPropertiesNames().Returns(new List<(string DisplayName, string PropertyName)>());
        var expected = new List<(string DisplayName, string PropertyName)>
        {
            ("Max points", "CourseTaskDto.MaxPoints"),
            ("Student email", "Student.UserEmail"),
            ("Student Name", "Student.LastName")
        };
        
        var propertiesNames = _sut.GetAllPropertiesNames();

        propertiesNames.Should().BeEquivalentTo(expected);
        _baseSelector.Received().GetAllPropertiesNames();
    }

    [Fact]
    public void GetAllProperties_ShouldAdd3MoreValues()
    {
        var submission = new TaskSubmissionDto
        {
            CourseTask = new CourseTaskDto { MaxPoints = 20 },
            Student = new StudentDto { LastName = "ln", FirstName = "fn", UserEmail = "email" }
        };
        _baseSelector.GetAllProperties(submission).Returns(new List<(string DisplayName, string PropertyName)>());
        var expected = new List<(string DisplayName, string Value)>
        {
            ("Max points", submission.CourseTask?.MaxPoints.ToString() ?? "-"),
            ("Email", "email"),
            ("Student Name", $"{submission.Student?.LastName} {submission.Student?.FirstName}")
        };

        var result = _sut.GetAllProperties(submission);

        result.Should().BeEquivalentTo(expected);
        _baseSelector.Received().GetAllProperties(submission);
    }
    
    [Fact]
    public void GetAllPropertiesValues_ShouldAdd3MoreValues()
    {
        var submission = new TaskSubmissionDto
        {
            CourseTask = new CourseTaskDto { MaxPoints = 20 },
            Student = new StudentDto { LastName = "ln", FirstName = "fn", UserEmail = "email" }
        };
        _baseSelector.GetAllPropertiesValues(submission).Returns(new List<string>());
        var expected = new List<string>
        {
            submission.CourseTask!.MaxPoints.ToString(),
            "email",
            $"{submission.Student.LastName} {submission.Student.FirstName}"
        };

        var result = _sut.GetAllPropertiesValues(submission);

        result.Should().BeEquivalentTo(expected);
        _baseSelector.Received().GetAllPropertiesValues(submission);
    }
}