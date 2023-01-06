using UniversityTimetable.Domain.Validation.FluentValidators;
using UniversityTimetable.Shared.DataTransferObjects;

namespace UniversityTimetable.Tests.Unit.BackEnd.Domain.Validation.FluentValidators;

public class FacultiesValidatorTests
{
    [Fact]
    public void PassedInvalidItem_ShouldHaveValidationError_ServiceCalled()
    {
        var invalidItem = new FacultyDto { Name = "" };
        var validator = new FacultyDtoValidator();

        var errors = validator.Validate(invalidItem).Errors.Select(e => e.ErrorMessage).ToList();
        var expectedErrors = new List<string>
        {
            "Please, enter name",
        };
        errors.Should().BeEquivalentTo(expectedErrors);
    }
}