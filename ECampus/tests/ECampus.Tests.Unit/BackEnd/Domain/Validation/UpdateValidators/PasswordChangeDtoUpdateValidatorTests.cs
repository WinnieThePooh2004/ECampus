using ECampus.Contracts.DataAccess;
using ECampus.Domain.Interfaces.Validation;
using ECampus.Domain.Validation.UpdateValidators;
using ECampus.Shared.DataTransferObjects;
using ECampus.Shared.Models;
using ECampus.Shared.Validation;

namespace ECampus.Tests.Unit.BackEnd.Domain.Validation.UpdateValidators;

public class PasswordChangeDtoUpdateValidatorTests
{
    private readonly PasswordChangeDtoUpdateValidator _sut;
    private readonly IUpdateValidator<PasswordChangeDto> _baseValidator;
    private readonly IDataAccessManager _dataAccessManager = Substitute.For<IDataAccessManager>();

    public PasswordChangeDtoUpdateValidatorTests()
    {
        var dataAccessManagerFactory = Substitute.For<IDataAccessManagerFactory>();
        dataAccessManagerFactory.Primitive.Returns(_dataAccessManager);
        _baseValidator = Substitute.For<IUpdateValidator<PasswordChangeDto>>();
        _sut = new PasswordChangeDtoUpdateValidator(_baseValidator, dataAccessManagerFactory);
    }

    [Fact]
    public async Task Validate_ShouldReturnMessage_WhenPasswordsNotMatches()
    {
        var passwordChange = new PasswordChangeDto
            { UserId = 10, NewPassword = "new", OldPassword = "old", NewPasswordConfirm = "new" };
        _baseValidator.ValidateAsync(passwordChange).Returns(new ValidationResult());
        _dataAccessManager.GetByIdAsync<User>(10).Returns(new User { Password = "" });

        var errors = await _sut.ValidateAsync(passwordChange);

        errors.ToList().Should().Contain(new ValidationError("OldPassword", "Invalid old password"));
    }
}