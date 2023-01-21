using ECampus.Domain.Validation.FluentValidators;
using ECampus.Shared.DataTransferObjects;

namespace ECampus.Tests.Unit.BackEnd.Domain.Validation.FluentValidators;

public class PasswordChangeValidatorTests
{
    [Fact]
    public void ValidateAsync_PassesInvalidObject_ShouldHaveValidationError()
    {
        var invalidItem = new PasswordChangeDto { UserId = 0, NewPassword = "*", OldPassword = "a" };
        var validator = new PasswordChangeDtoValidator();

        var errors = validator.Validate(invalidItem).Errors.Select(e => e.ErrorMessage).ToList();
        var expectedErrors = new List<string>
        {
            "New password must be at least 8 symbols",
            "New password must contain at least one uppercase letter.",
            "New password must contain at least one lowercase letter.",
            "New password must contain at least one number.",
            "Passwords don't match"
        };
        
        errors.Should().BeEquivalentTo(expectedErrors);
    }
}