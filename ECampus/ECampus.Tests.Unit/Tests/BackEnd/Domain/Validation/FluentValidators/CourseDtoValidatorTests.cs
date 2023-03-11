using ECampus.Domain.DataTransferObjects;
using ECampus.Validation;

namespace ECampus.Tests.Unit.Tests.BackEnd.Domain.Validation.FluentValidators;

public class CourseDtoValidatorTests
{
    private readonly CourseDtoValidator _sut = new ();

    [Fact]
    public void Validate_ShouldReturnValidationErrors_WhenInvalidObjectPassed()
    {
        var expectedErrors = new List<string>
        {
            "Course name cannot be empty",
            "Course must have at least one teacher",
            "Course must have at least one group"
        };

        var actualErrors = _sut.Validate(new CourseDto()).Errors.Select(e => e.ErrorMessage);

        actualErrors.Should().BeEquivalentTo(expectedErrors);
    }
}