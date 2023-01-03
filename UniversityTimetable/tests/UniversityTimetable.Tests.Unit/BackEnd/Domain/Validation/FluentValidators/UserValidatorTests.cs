using UniversityTimetable.Domain.Validation.FluentValidators;
using UniversityTimetable.Shared.DataTransferObjects;

namespace UniversityTimetable.Tests.Unit.BackEnd.Domain.Validation.FluentValidators;

public class UserValidatorTests
{
    [Fact]
    public void Validate_InvalidItemPasses_ShouldHaveValidationError()
    {
        var invalidItem = new UserDto { Username = "", Email = "", Password = "", PasswordConfirm = "a" };
        var validator = new UserDtoValidator();

        var errors = validator.Validate(invalidItem).Errors.Select(e => e.ErrorMessage).ToList();
        var expectedErrors = new List<string>
        {
            "Enter valid email",
            "Password must be at least 8 symbols",
            "Enter some password, please.",
            "Password length must be at least 8 characters.",
            "Password must contain at least one uppercase letter.",
            "Password must contain at least one lowercase letter.",
            "Password must contain at least one number.",
            "Passwords don't match"
        };
        errors.Should().BeEquivalentTo(expectedErrors);
    }
}