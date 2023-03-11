using ECampus.Domain.DataTransferObjects;
using ECampus.Validation;

namespace ECampus.Tests.Unit.Tests.BackEnd.Domain.Validation.FluentValidators;

public class CourseTaskDtoValidatorTests
{
    private readonly CourseTaskDtoValidator _sut = new();

    [Fact]
    public void Validate_ShouldReturnValidationErrors_WhenNameIsEmpty()
    {
        var expectedErrors = new List<string>
        {
            "Student id cannot be 0",
            "Task name cannot be empty",
            "Coefficient must be more than 0"
        };

        var actualErrors = _sut.Validate(new CourseTaskDto { Coefficient = -1 })
            .Errors.Select(e => e.ErrorMessage);

        actualErrors.Should().BeEquivalentTo(expectedErrors);
    }

    [Fact]
    public void Validate_ShouldReturnValidationErrors_WhenNameToLong()
    {
        var expectedErrors = new List<string>
        {
            "Student id cannot be 0",
            "Task name cannot be longer than 40 symbols",
            "Coefficient must not be greater than 0"
        };
        var invalidName = new char[41];

        var actualErrors = _sut.Validate(
                new CourseTaskDto { Name = new string(invalidName), Coefficient = 10 }).Errors
            .Select(e => e.ErrorMessage);

        actualErrors.Should().BeEquivalentTo(expectedErrors);
    }
}