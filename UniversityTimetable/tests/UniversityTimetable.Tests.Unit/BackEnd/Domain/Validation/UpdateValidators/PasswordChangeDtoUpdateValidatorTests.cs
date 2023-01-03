using UniversityTimetable.Domain.Validation.UpdateValidators;
using UniversityTimetable.Shared.DataTransferObjects;
using UniversityTimetable.Shared.Interfaces.Data.Validation;
using UniversityTimetable.Shared.Models;

namespace UniversityTimetable.Tests.Unit.BackEnd.Domain.Validation.UpdateValidators;

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
        _baseValidator.ValidateAsync(passwordChange).Returns(new List<KeyValuePair<string, string>>());
        _dataAccess.LoadRequiredDataForUpdate(Arg.Any<User>()).Returns(new User{ Password = ""});

        var errors = await _sut.ValidateAsync(passwordChange);

        errors.Should().Contain(KeyValuePair.Create("OldPassword", "Invalid old password"));
    }
}