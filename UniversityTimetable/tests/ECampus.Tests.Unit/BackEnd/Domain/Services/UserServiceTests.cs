using ECampus.Domain.Services;
using ECampus.Shared.DataTransferObjects;
using ECampus.Shared.Exceptions.DomainExceptions;
using ECampus.Shared.Interfaces.DataAccess;
using ECampus.Shared.Interfaces.Domain;
using ECampus.Shared.Interfaces.Domain.Validation;
using ECampus.Shared.Validation;

namespace ECampus.Tests.Unit.BackEnd.Domain.Services;

public class UserServiceTests
{
    private readonly UserService _sut;
    private readonly IUpdateValidator<PasswordChangeDto> _passwordChangeValidator;
    private readonly IUserDataAccessFacade _userDataAccessFacade;
    private readonly Fixture _fixture = new();
    private readonly ICreateValidator<UserDto> _createValidator = Substitute.For<ICreateValidator<UserDto>>();
    private readonly IUpdateValidator<UserDto> _updateValidator = Substitute.For<IUpdateValidator<UserDto>>();

    public UserServiceTests()
    {
        _passwordChangeValidator = Substitute.For<IUpdateValidator<PasswordChangeDto>>();
        Substitute.For<IBaseService<UserDto>>();
        _userDataAccessFacade = Substitute.For<IUserDataAccessFacade>();

        _sut = new UserService(_passwordChangeValidator, _userDataAccessFacade, _updateValidator,
            _createValidator);
    }

    [Fact]
    public async Task ValidatePasswordChange_ShouldReturnFromPasswordChange()
    {
        var passwordChange = new PasswordChangeDto();
        var errors = new ValidationResult(_fixture.CreateMany<ValidationError>(10).ToList());
        _passwordChangeValidator.ValidateAsync(passwordChange).Returns(errors);

        var result = await _sut.ValidatePasswordChange(passwordChange);

        result.Should().Be(errors);
    }

    [Fact]
    private async Task ChangePassword_ShouldThrowValidationException_WhenHasValidationError()
    {
        var errors = new ValidationResult(new ValidationError("", ""));
        var passwordChange = _fixture.Create<PasswordChangeDto>();
        _passwordChangeValidator.ValidateAsync(passwordChange).Returns(errors);

        await new Func<Task>(() => _sut.ChangePassword(passwordChange)).Should().ThrowAsync<ValidationException>()
            .WithMessage(new ValidationException(typeof(PasswordChangeDto), errors).Message);

        await _userDataAccessFacade.DidNotReceive().ChangePassword(Arg.Any<PasswordChangeDto>());
    }

    [Fact]
    private async Task ChangePassword_ShouldReturnDtoBack_ShouldCallPasswordChange()
    {
        var errors = new ValidationResult();
        var passwordChange = _fixture.Create<PasswordChangeDto>();
        _passwordChangeValidator.ValidateAsync(passwordChange).Returns(errors);

        var result = await _sut.ChangePassword(passwordChange);

        result.Should().Be(passwordChange);
        await _userDataAccessFacade.Received(1).ChangePassword(passwordChange);
    }

    [Fact]
    private async Task ValidateCreate_ShouldReturnFromValidationFacade()
    {
        var errors = new ValidationResult(_fixture.CreateMany<ValidationError>(5).ToList());
        var user = new UserDto();
        _createValidator.ValidateAsync(user).Returns(errors);

        var result = await _sut.ValidateCreateAsync(user);

        result.Should().BeEquivalentTo(errors);
        await _createValidator.Received(1).ValidateAsync(user);
    }

    [Fact]
    private async Task ValidateUpdate_ShouldReturnFromValidationFacade()
    {
        var errors = new ValidationResult(_fixture.CreateMany<ValidationError>(5).ToList());
        var user = new UserDto();
        _updateValidator.ValidateAsync(user).Returns(errors);

        var result = await _sut.ValidateUpdateAsync(user);

        result.Should().BeEquivalentTo(errors);
        await _updateValidator.Received(1).ValidateAsync(user);
    }
}