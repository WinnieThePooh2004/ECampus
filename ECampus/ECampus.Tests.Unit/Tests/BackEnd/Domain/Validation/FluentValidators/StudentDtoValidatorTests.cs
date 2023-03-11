using ECampus.Domain.DataTransferObjects;
using ECampus.Validation;

namespace ECampus.Tests.Unit.Tests.BackEnd.Domain.Validation.FluentValidators;

public class StudentDtoValidatorTests
{
    [Fact]
    public void PassedInvalidItem_ShouldHaveValidationError()
    {
        var invalidItem = new StudentDto();
        var validator = new StudentDtoValidator();

        var errors = validator.Validate(invalidItem).Errors.Select(e => e.ErrorMessage).ToList();
        var expectedErrors = new List<string>
        {
            "Please, enter student`s first name",
            "Please, enter student`s last name",
            "Please, enter student`s group"
        };
        errors.Should().BeEquivalentTo(expectedErrors);
    }
}