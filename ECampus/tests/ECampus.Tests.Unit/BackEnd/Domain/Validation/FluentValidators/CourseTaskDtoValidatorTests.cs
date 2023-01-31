using ECampus.Domain.Validation.FluentValidators;
using ECampus.Shared.DataTransferObjects;

namespace ECampus.Tests.Unit.BackEnd.Domain.Validation.FluentValidators;

public class CourseTaskDtoValidatorTests
{
    private readonly CourseTaskDtoValidator _sut = new();

    [Fact]
    public void Validate_ShouldReturnValidationErrors_WhenNameIsEmpty()
    {
        var expectedErrors = new List<string>
        {
            "Student id cannot be 0",
            "Task name cannot be empty"
        };

        var actualErrors = _sut.Validate(new CourseTaskDto()).Errors.Select(e => e.ErrorMessage);

        actualErrors.Should().BeEquivalentTo(expectedErrors);
    }

    [Fact]
    public void Validate_ShouldReturnValidationErrors_WhenNameToLong()
    {
        var expectedErrors = new List<string>
        {
            "Student id cannot be 0",
            "Task name cannot be longer than 40 symbols"
        };
        var invalidName = new char[41];

        var actualErrors = _sut.Validate(new CourseTaskDto { Name = new string(invalidName) }).Errors
            .Select(e => e.ErrorMessage);

        actualErrors.Should().BeEquivalentTo(expectedErrors);
    }
}