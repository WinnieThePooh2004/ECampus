using UniversityTimetable.Domain.Validation.FluentValidators;
using UniversityTimetable.Shared.DataTransferObjects;

namespace UniversityTimetable.Tests.Unit.BackEnd.Domain.Validation.FluentValidators;

public class TeacherValidatorTests
{
    [Fact]
    public void PassedInvalidItem_ShouldHaveValidationError()
    {
        var invalidItem = new TeacherDto { FirstName = "", LastName = "" };
        var validator = new TeacherDtoValidator();

        var errors = validator.Validate(invalidItem).Errors.Select(e => e.ErrorMessage).ToList();
        var expectedErrors = new List<string>
        {
            "Please, enter first name",
            "Please, enter last name"
        };
        errors.Should().BeEquivalentTo(expectedErrors);
    }
}