using ECampus.Domain.DataTransferObjects;
using ECampus.Validation;

namespace ECampus.Tests.Unit.Tests.BackEnd.Domain.Validation.FluentValidators;

public class UserValidatorTests
{
    [Fact]
    public void Validate_InvalidItemPasses_ShouldHaveValidationError()
    {
        var invalidItem = new UserDto { Username = "", Email = "" };
        var validator = new UserDtoValidator();

        var errors = validator.Validate(invalidItem).Errors.Select(e => e.ErrorMessage).ToList();
        var expectedErrors = new List<string>
        {
            "Enter valid email"
        };
        errors.Should().BeEquivalentTo(expectedErrors);
    }
}