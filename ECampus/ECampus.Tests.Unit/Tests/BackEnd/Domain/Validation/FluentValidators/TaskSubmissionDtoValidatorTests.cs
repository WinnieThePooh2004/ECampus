using ECampus.Domain.DataTransferObjects;
using ECampus.Validation;

namespace ECampus.Tests.Unit.Tests.BackEnd.Domain.Validation.FluentValidators;

public class TaskSubmissionDtoValidatorTests
{
    private readonly TaskSubmissionDtoValidator _sut = new();

    [Fact]
    public void Validate_ShouldReturnValidationErrors_WhenNameIsEmpty()
    {
        var expectedErrors = new List<string>
        {
            "Submission must contain not more than 450 symbols",
            "Mark cannot be less than 0"
        };
        var invalidInstance = new TaskSubmissionDto { TotalPoints = -1, SubmissionContent = new string(new char[451]) };
        
        var actualErrors = _sut.Validate(invalidInstance).Errors.Select(e => e.ErrorMessage);

        actualErrors.Should().BeEquivalentTo(expectedErrors);
    }

    [Fact]
    public void Validate_ShouldReturnValidationErrors_WhenCourseTaskIsNotNullAndMarkIsTooHigh()
    {
        var expectedErrors = new List<string>
        {
            "Mark cannot be more than maximal mark for this task"
        };
        var invalidInstance = new TaskSubmissionDto { TotalPoints = 100, CourseTask = new CourseTaskDto{MaxPoints = 10}};

        var actualErrors = _sut.Validate(invalidInstance).Errors.Select(e => e.ErrorMessage);

        actualErrors.Should().BeEquivalentTo(expectedErrors);
    }
}