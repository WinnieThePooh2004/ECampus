using System.Security.Claims;
using ECampus.DataAccess.Contracts.DataAccess;
using ECampus.DataAccess.Contracts.DataSelectParameters;
using ECampus.Domain.DataTransferObjects;
using ECampus.Domain.Entities;
using ECampus.Domain.Validation;
using ECampus.Services.Contracts.Validation;
using ECampus.Services.Validation.UpdateValidators;
using ECampus.Tests.Shared.Mocks.EntityFramework;
using Microsoft.AspNetCore.Http;

namespace ECampus.Tests.Unit.Tests.BackEnd.Domain.Validation.UpdateValidators;

public class PasswordChangeDtoUpdateValidatorTests
{
    private readonly PasswordChangeDtoUpdateValidator _sut;
    private readonly IUpdateValidator<PasswordChangeDto> _baseValidator;
    private readonly IDataAccessFacade _dataAccessFacade = Substitute.For<IDataAccessFacade>();
    private readonly ClaimsPrincipal _user = Substitute.For<ClaimsPrincipal>();

    public PasswordChangeDtoUpdateValidatorTests()
    {
        _baseValidator = Substitute.For<IUpdateValidator<PasswordChangeDto>>();
        var accessor = Substitute.For<HttpContextAccessor>();
        accessor.HttpContext = Substitute.For<HttpContext>();
        accessor.HttpContext.User = _user;
        _sut = new PasswordChangeDtoUpdateValidator(_baseValidator, _dataAccessFacade, accessor);
    }

    [Fact]
    public async Task Validate_ShouldReturnMessage_WhenPasswordsNotMatches()
    {
        _user.FindFirst(ClaimTypes.Sid).Returns(new Claim("10", "10"));
        var passwordChange = new PasswordChangeDto
            { UserId = 10, NewPassword = "new", OldPassword = "old", NewPasswordConfirm = "new" };
        _baseValidator.ValidateAsync(passwordChange).Returns(new ValidationResult());
        var users = new DbSetMock<User>(new User { Password = "" }).Object;
        _dataAccessFacade.GetByParameters<User, PureByIdParameters<User>>(Arg.Any<PureByIdParameters<User>>())
            .Returns(users);

        var errors = await _sut.ValidateAsync(passwordChange);

        errors.ToList().Should().Contain(new ValidationError("OldPassword", "Invalid old password"));
    }

    [Fact]
    public async Task Validate_ShouldAddError_WhenIdsNotMatch()
    {
        var errors = new ValidationResult(new ValidationError("abc", "bcd"));
        var token = new CancellationToken(); 
        _user.FindFirst(ClaimTypes.Sid).Returns(new Claim("", "10"));
        _baseValidator.ValidateAsync(Arg.Any<PasswordChangeDto>(), token).Returns(errors);
        var expectedErrors = new ValidationResult(errors);
        expectedErrors.AddError(new ValidationError("UserId",
            "You must be account owner or admin to change this user`s password"));

        var result = await _sut.ValidateAsync(new PasswordChangeDto { UserId = 200 }, token);

        _dataAccessFacade.ReceivedCalls().Should().BeEmpty();
        result.Should().BeEquivalentTo(expectedErrors);
    }
}