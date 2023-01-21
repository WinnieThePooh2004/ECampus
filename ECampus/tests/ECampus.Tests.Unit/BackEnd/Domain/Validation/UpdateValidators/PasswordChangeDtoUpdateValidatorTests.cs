using ECampus.Domain.Validation.UpdateValidators;
using ECampus.Shared.DataTransferObjects;
using ECampus.Shared.Interfaces.Domain.Validation;
using ECampus.Shared.Models;
using ECampus.Shared.Validation;

namespace ECampus.Tests.Unit.BackEnd.Domain.Validation.UpdateValidators;

public class PasswordChangeDtoUpdateValidatorTests
{
    private readonly PasswordChangeDtoUpdateValidator _sut;
    private readonly IUpdateValidator<PasswordChangeDto> _baseValidator;
    private readonly IValidationDataAccess<User> _dataAccess;

    public PasswordChangeDtoUpdateValidatorTests()
    {
        _baseValidator = Substitute.For<IUpdateValidator<PasswordChangeDto>>();
        _dataAccess = Substitute.For<IValidationDataAccess<User>>();
        _sut = new PasswordChangeDtoUpdateValidator(_baseValidator, _dataAccess);
    }

    [Fact]
    public async Task Validate_ShouldReturnMessage_WhenPasswordsNotMatches()
    {
        var passwordChange = new PasswordChangeDto
            { UserId = 10, NewPassword = "new", OldPassword = "old", NewPasswordConfirm = "new" };
        _baseValidator.ValidateAsync(passwordChange).Returns(new ValidationResult());
        _dataAccess.LoadRequiredDataForUpdateAsync(Arg.Any<User>()).Returns(new User { Password = "" });

        var errors = await _sut.ValidateAsync(passwordChange);

        errors.GetAllErrors().Should().Contain(new ValidationError("OldPassword", "Invalid old password"));
    }
}