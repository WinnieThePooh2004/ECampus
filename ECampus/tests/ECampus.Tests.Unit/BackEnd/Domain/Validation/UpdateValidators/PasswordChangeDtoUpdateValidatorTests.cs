using System.Security.Claims;
using ECampus.Contracts.DataAccess;
using ECampus.Contracts.DataSelectParameters;
using ECampus.Domain.Interfaces.Validation;
using ECampus.Domain.Validation.UpdateValidators;
using ECampus.Shared.Auth;
using ECampus.Shared.DataTransferObjects;
using ECampus.Shared.Models;
using ECampus.Shared.Validation;
using ECampus.Tests.Shared.Mocks.EntityFramework;
using Microsoft.AspNetCore.Http;

namespace ECampus.Tests.Unit.BackEnd.Domain.Validation.UpdateValidators;

public class PasswordChangeDtoUpdateValidatorTests
{
    private readonly PasswordChangeDtoUpdateValidator _sut;
    private readonly IUpdateValidator<PasswordChangeDto> _baseValidator;
    private readonly IDataAccessManager _dataAccessManager = Substitute.For<IDataAccessManager>();
    private readonly ClaimsPrincipal _user = Substitute.For<ClaimsPrincipal>();

    public PasswordChangeDtoUpdateValidatorTests()
    {
        _baseValidator = Substitute.For<IUpdateValidator<PasswordChangeDto>>();
        var accessor = Substitute.For<HttpContextAccessor>();
        accessor.HttpContext = Substitute.For<HttpContext>();
        accessor.HttpContext.User = _user;
        _sut = new PasswordChangeDtoUpdateValidator(_baseValidator, _dataAccessManager, accessor);
    }

    [Fact]
    public async Task Validate_ShouldReturnMessage_WhenPasswordsNotMatches()
    {
        _user.FindFirst(CustomClaimTypes.Id).Returns(new Claim("10", "10"));
        var passwordChange = new PasswordChangeDto
            { UserId = 10, NewPassword = "new", OldPassword = "old", NewPasswordConfirm = "new" };
        _baseValidator.ValidateAsync(passwordChange).Returns(new ValidationResult());
        var users = new DbSetMock<User>(new User { Password = "" }).Object;
        _dataAccessManager.GetByParameters<User, PureByIdParameters<User>>(Arg.Any<PureByIdParameters<User>>())
            .Returns(users);

        var errors = await _sut.ValidateAsync(passwordChange);

        errors.ToList().Should().Contain(new ValidationError("OldPassword", "Invalid old password"));
    }

    [Fact]
    public async Task Validate_ShouldAddError_WhenIdsNotMatch()
    {
        var errors = new ValidationResult(new ValidationError("abc", "bcd"));
        var token = new CancellationToken(); 
        _user.FindFirst(CustomClaimTypes.Id).Returns(new Claim("", "10"));
        _baseValidator.ValidateAsync(Arg.Any<PasswordChangeDto>(), token).Returns(errors);
        var expectedErrors = new ValidationResult(errors);
        expectedErrors.AddError(new ValidationError("UserId",
            "You must be account owner or admin to change this user`s password"));

        var result = await _sut.ValidateAsync(new PasswordChangeDto { UserId = 200 }, token);

        _dataAccessManager.ReceivedCalls().Should().BeEmpty();
        result.Should().BeEquivalentTo(expectedErrors);
    }
}