using UniversityTimetable.Domain.Services;
using UniversityTimetable.Shared.DataTransferObjects;
using UniversityTimetable.Shared.Exceptions.DomainExceptions;
using UniversityTimetable.Shared.Interfaces.Auth;
using UniversityTimetable.Shared.Interfaces.Domain.Validation;
using UniversityTimetable.Shared.Interfaces.DataAccess;
using UniversityTimetable.Shared.Interfaces.Domain;

namespace UniversityTimetable.Tests.Unit.BackEnd.Domain.Services;

public class UserServiceTests
{
    private readonly UserService _sut;
    private readonly IUpdateValidator<PasswordChangeDto> _passwordChangeValidator;
    private readonly IBaseService<UserDto> _baseService;
    private readonly IUserDataAccessFacade _userDataAccessFacade;
    private readonly Fixture _fixture = new();
    private readonly ICreateValidator<UserDto> _createValidator = Substitute.For<ICreateValidator<UserDto>>();
    private readonly IUpdateValidator<UserDto> _updateValidator = Substitute.For<IUpdateValidator<UserDto>>();

    public UserServiceTests()
    {
        var authenticationService = Substitute.For<IAuthenticationService>();
        _passwordChangeValidator = Substitute.For<IUpdateValidator<PasswordChangeDto>>();
        _baseService = Substitute.For<IBaseService<UserDto>>();
        _userDataAccessFacade = Substitute.For<IUserDataAccessFacade>();

        _sut = new UserService(_baseService,
            authenticationService, _passwordChangeValidator, _userDataAccessFacade, _updateValidator, _createValidator);
    }

    [Fact]
    public async Task Create_ReturnsFromBaseService()
    {
        var user = CreateUser();
        _baseService.CreateAsync(user).Returns(user);

        var result = await _sut.CreateAsync(user);

        result.Should().Be(user);
        await _baseService.Received(1).CreateAsync(user);
    }

    [Fact]
    public async Task Update_ReturnsFromBaseService()
    {
        var user = CreateUser();
        _baseService.UpdateAsync(user).Returns(user);

        var result = await _sut.UpdateAsync(user);

        result.Should().Be(user);
        await _baseService.Received(1).UpdateAsync(user);
    }

    [Fact]
    public async Task GetById_ReturnsFromBaseService()
    {
        var user = CreateUser();
        _baseService.GetByIdAsync(10).Returns(user);

        var result = await _sut.GetByIdAsync(10);

        result.Should().Be(user);
        await _baseService.Received(1).GetByIdAsync(10);
    }

    [Fact]
    private async Task Delete_BaseServiceCalled()
    {
        await _sut.DeleteAsync(10);

        await _baseService.Received(1).DeleteAsync(10);
    }

    [Fact]
    private async Task SaveGroup_RelationsRepositoryCalled()
    {
        await _sut.SaveGroup(10, 10);

        await _userDataAccessFacade.Received(1).SaveGroup(10, 10);
    }

    [Fact]
    private async Task SaveAuditory_RelationsRepositoryCalled()
    {
        await _sut.SaveAuditory(10, 10);

        await _userDataAccessFacade.Received(1).SaveAuditory(10, 10);
    }

    [Fact]
    private async Task SaveTeacher_RelationsRepositoryCalled()
    {
        await _sut.SaveTeacher(10, 10);

        await _userDataAccessFacade.Received(1).SaveTeacher(10, 10);
    }

    [Fact]
    private async Task RemoveSavedGroup_RelationsRepositoryCalled()
    {
        await _sut.RemoveSavedGroup(10, 10);

        await _userDataAccessFacade.Received(1).RemoveSavedGroup(10, 10);
    }

    [Fact]
    private async Task RemoveSavedAuditory_RelationsRepositoryCalled()
    {
        await _sut.RemoveSavedAuditory(10, 10);

        await _userDataAccessFacade.Received(1).RemoveSavedAuditory(10, 10);
    }

    [Fact]
    private async Task RemoveSavedTeacher_RelationsRepositoryCalled()
    {
        await _sut.RemoveSavedTeacher(10, 10);

        await _userDataAccessFacade.Received(1).RemoveSavedTeacher(10, 10);
    }

    [Fact]
    private async Task ChangePassword_ShouldThrowValidationException_WhenHasValidationError()
    {
        var errors = new List<KeyValuePair<string, string>> { KeyValuePair.Create("", "") };
        var passwordChange = _fixture.Create<PasswordChangeDto>();
        _passwordChangeValidator.ValidateAsync(passwordChange).Returns(errors);

        await new Func<Task>(() => _sut.ChangePassword(passwordChange)).Should().ThrowAsync<ValidationException>()
            .WithMessage(new ValidationException(typeof(PasswordChangeDto), errors).Message);

        await _userDataAccessFacade.DidNotReceive().ChangePassword(Arg.Any<PasswordChangeDto>());
    }

    [Fact]
    private async Task ChangePassword_ShouldReturnDtoBack_ShouldCallPasswordChange()
    {
        var errors = new List<KeyValuePair<string, string>>();
        var passwordChange = _fixture.Create<PasswordChangeDto>();
        _passwordChangeValidator.ValidateAsync(passwordChange).Returns(errors);

        var result = await _sut.ChangePassword(passwordChange);

        result.Should().Be(passwordChange);
        await _userDataAccessFacade.Received(1).ChangePassword(passwordChange);
    }

    [Fact]
    private async Task ValidateCreate_ShouldReturnFromValidationFacade()
    {
        var errors = _fixture.CreateMany<KeyValuePair<string, string>>(5).ToList();
        var user = new UserDto();
        _createValidator.ValidateAsync(user).Returns(errors);

        var result = await _sut.ValidateCreateAsync(user);

        result.Should().BeEquivalentTo(errors);
        await _createValidator.Received(1).ValidateAsync(user);
    }

    [Fact]
    private async Task ValidateUpdate_ShouldReturnFromValidationFacade()
    {
        var errors = _fixture.CreateMany<KeyValuePair<string, string>>(5).ToList();
        var user = new UserDto();
        _updateValidator.ValidateAsync(user).Returns(errors);

        var result = await _sut.ValidateUpdateAsync(user);

        result.Should().BeEquivalentTo(errors);
        await _updateValidator.Received(1).ValidateAsync(user);
    }
    
    private UserDto CreateUser() =>
        _fixture.Build<UserDto>()
            .Without(u => u.SavedAuditories)
            .Without(u => u.SavedGroups)
            .Without(u => u.SavedTeachers)
            .Create();
}