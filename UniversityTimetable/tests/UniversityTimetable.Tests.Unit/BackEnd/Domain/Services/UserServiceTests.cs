using UniversityTimetable.Domain.Services;
using UniversityTimetable.Shared.DataTransferObjects;
using UniversityTimetable.Shared.Exceptions.DomainExceptions;
using UniversityTimetable.Shared.Interfaces.Auth;
using UniversityTimetable.Shared.Interfaces.Data.DataServices;
using UniversityTimetable.Shared.Interfaces.Data.Validation;
using UniversityTimetable.Shared.Interfaces.DataAccess;
using UniversityTimetable.Shared.Interfaces.Domain;
using UniversityTimetable.Shared.Models;
using UniversityTimetable.Shared.Models.RelationModels;

namespace UniversityTimetable.Tests.Unit.BackEnd.Domain.Services;

public class UserServiceTests
{
    private readonly UserService _sut;
    private readonly IRelationsDataAccess<User, Auditory, UserAuditory> _userAuditoryRelations;
    private readonly IRelationsDataAccess<User, Group, UserGroup> _userGroupRelations;
    private readonly IRelationsDataAccess<User, Teacher, UserTeacher> _userTeacherRelations;
    private readonly IUpdateValidator<PasswordChangeDto> _passwordChangeValidator;
    private readonly IValidationFacade<UserDto> _userValidator;
    private readonly IBaseService<UserDto> _baseService;
    private readonly IPasswordChange _passwordChange;
    private readonly Fixture _fixture = new();

    public UserServiceTests()
    {
        _userValidator = Substitute.For<IValidationFacade<UserDto>>();
        var authenticationService = Substitute.For<IAuthenticationService>();
        _passwordChangeValidator = Substitute.For<IUpdateValidator<PasswordChangeDto>>();
        _userAuditoryRelations = Substitute.For<IRelationsDataAccess<User, Auditory, UserAuditory>>();
        _userGroupRelations = Substitute.For<IRelationsDataAccess<User, Group, UserGroup>>();
        _userTeacherRelations = Substitute.For<IRelationsDataAccess<User, Teacher, UserTeacher>>();
        _baseService = Substitute.For<IBaseService<UserDto>>();
        _passwordChange = Substitute.For<IPasswordChange>();

        _sut = new UserService(_baseService, _userAuditoryRelations, _userGroupRelations, _userTeacherRelations,
            authenticationService, _passwordChangeValidator, _userValidator, _passwordChange);
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

        await _userGroupRelations.Received(1).CreateRelation(10, 10);
    }

    [Fact]
    private async Task SaveAuditory_RelationsRepositoryCalled()
    {
        await _sut.SaveAuditory(10, 10);

        await _userAuditoryRelations.Received(1).CreateRelation(10, 10);
    }

    [Fact]
    private async Task SaveTeacher_RelationsRepositoryCalled()
    {
        await _sut.SaveTeacher(10, 10);

        await _userTeacherRelations.Received(1).CreateRelation(10, 10);
    }

    [Fact]
    private async Task RemoveSavedGroup_RelationsRepositoryCalled()
    {
        await _sut.RemoveSavedGroup(10, 10);

        await _userGroupRelations.Received(1).DeleteRelation(10, 10);
    }

    [Fact]
    private async Task RemoveSavedAuditory_RelationsRepositoryCalled()
    {
        await _sut.RemoveSavedAuditory(10, 10);

        await _userAuditoryRelations.Received(1).DeleteRelation(10, 10);
    }

    [Fact]
    private async Task RemoveSavedTeacher_RelationsRepositoryCalled()
    {
        await _sut.RemoveSavedTeacher(10, 10);

        await _userTeacherRelations.Received(1).DeleteRelation(10, 10);
    }

    [Fact]
    private async Task ChangePassword_ShouldThrowValidationException_WhenHasValidationError()
    {
        var errors = new List<KeyValuePair<string, string>> { KeyValuePair.Create("", "") };
        var passwordChange = _fixture.Create<PasswordChangeDto>();
        _passwordChangeValidator.ValidateAsync(passwordChange).Returns(errors);

        await new Func<Task>(() => _sut.ChangePassword(passwordChange)).Should().ThrowAsync<ValidationException>()
            .WithMessage(new ValidationException(typeof(PasswordChangeDto), errors).Message);

        await _passwordChange.DidNotReceive().ChangePassword(Arg.Any<PasswordChangeDto>());
    }

    [Fact]
    private async Task ChangePassword_ShouldReturnDtoBack_ShouldCallPasswordChange()
    {
        var errors = new List<KeyValuePair<string, string>>();
        var passwordChange = _fixture.Create<PasswordChangeDto>();
        _passwordChangeValidator.ValidateAsync(passwordChange).Returns(errors);

        var result = await _sut.ChangePassword(passwordChange);

        result.Should().Be(passwordChange);
        await _passwordChange.Received(1).ChangePassword(passwordChange);
    }

    [Fact]
    private async Task ValidateCreate_ShouldReturnFromValidationFacade()
    {
        var errors = _fixture.CreateMany<KeyValuePair<string, string>>(5).ToList();
        var user = new UserDto();
        _userValidator.ValidateCreate(user).Returns(errors);

        var result = await _sut.ValidateCreateAsync(user);

        result.Should().BeEquivalentTo(errors);
        await _userValidator.Received(1).ValidateCreate(user);
    }

    [Fact]
    private async Task ValidateUpdate_ShouldReturnFromValidationFacade()
    {
        var errors = _fixture.CreateMany<KeyValuePair<string, string>>(5).ToList();
        var user = new UserDto();
        _userValidator.ValidateUpdate(user).Returns(errors);

        var result = await _sut.ValidateUpdateAsync(user);

        result.Should().BeEquivalentTo(errors);
        await _userValidator.Received(1).ValidateUpdate(user);
    }
    
    private UserDto CreateUser() =>
        _fixture.Build<UserDto>()
            .Without(u => u.SavedAuditories)
            .Without(u => u.SavedGroups)
            .Without(u => u.SavedTeachers)
            .Create();
}