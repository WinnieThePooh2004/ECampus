using System.Security.Claims;
using ECampus.Contracts.DataAccess;
using ECampus.Contracts.DataSelectParameters;
using ECampus.Domain.Interfaces.Validation;
using ECampus.Domain.Validation.UpdateValidators;
using ECampus.Shared.Auth;
using ECampus.Shared.DataTransferObjects;
using ECampus.Shared.Enums;
using ECampus.Shared.Models;
using ECampus.Shared.Validation;
using ECampus.Tests.Shared.Mocks.EntityFramework;
using ECampus.Tests.Unit.Extensions;
using Microsoft.AspNetCore.Http;

namespace ECampus.Tests.Unit.BackEnd.Domain.Validation.UpdateValidators;

public class UserUpdateValidatorTests
{
    private readonly UserUpdateValidator _sut;
    private readonly IUpdateValidator<UserDto> _baseValidator;
    private readonly IHttpContextAccessor _httpContextAccessor = Substitute.For<IHttpContextAccessor>();
    private readonly IDataAccessManager _dataAccess = Substitute.For<IDataAccessManager>();
    private readonly Fixture _fixture = new();
    private readonly ClaimsPrincipal _user = Substitute.For<ClaimsPrincipal>();

    public UserUpdateValidatorTests()
    {
        _baseValidator = Substitute.For<IUpdateValidator<UserDto>>();
        _httpContextAccessor.HttpContext = Substitute.For<HttpContext>();
        _httpContextAccessor.HttpContext.User = _user;

        _sut = new UserUpdateValidator(_baseValidator, _httpContextAccessor, _dataAccess);
    }

    [Fact]
    public async Task Validate_ReturnsFromBaseValidator_WhenBaseErrorsNotValid()
    {
        var baseErrors = _fixture.CreateMany<ValidationError>(10).ToList();
        var user = new UserDto { Email = "abc@example.com", Password = "Password" };
        _baseValidator.ValidateAsync(user).Returns(new ValidationResult(baseErrors));

        var actualErrors = (await _sut.ValidateAsync(user)).ToList();

        actualErrors.Should().BeEquivalentTo(baseErrors);
    }

    [Fact]
    public async Task Validate_ShouldAddError_WhenAdminTryToChangeHisRole()
    {
        var user = new UserDto
            { Id = 10, Role = UserRole.Guest, Email = "email", Password = "password", Username = "username" };
        _dataAccess.SetReturnById(10, new User { Role = UserRole.Admin });
        var userByUsername = new DbSetMock<User>(new User()).Object;
        _dataAccess.GetByParameters<User, UserUsernameParameters>(
                Arg.Is<UserUsernameParameters>(parameters => parameters.Username == user.Username))
            .Returns(userByUsername);
        _user.FindFirst(CustomClaimTypes.Id).Returns(new Claim("", "10"));
        _baseValidator.ValidateAsync(user).Returns(new ValidationResult());
        var expectedErrors = new List<ValidationError>
        {
            new(nameof(user.Role), "Admin cannon change role for him/herself"),
            new(nameof(user.Username), "User with this username already exists"),
            new(nameof(user.Email), "You cannot change email"),
            new(nameof(user.Password), "To change password use action 'Users/ChangePassword'")
        };

        var result = await _sut.ValidateAsync(user);

        result.Should().BeEquivalentTo(expectedErrors);
    }

    [Fact]
    public async Task Validate_ShouldAddError_WhenNotAdminTryToChangeRole()
    {
        var user = new UserDto
            { Id = 10, Role = UserRole.Admin, Email = "email", Password = "password", Username = "username" };
        _dataAccess.SetReturnById(10, new User { Role = UserRole.Guest });
        var userByUsername = new DbSetMock<User>(new User()).Object;
        _dataAccess.GetByParameters<User, UserUsernameParameters>(
                Arg.Is<UserUsernameParameters>(parameters => parameters.Username == user.Username))
            .Returns(userByUsername);
        _user.FindFirst(CustomClaimTypes.Id).Returns(new Claim("", "15"));
        _baseValidator.ValidateAsync(user).Returns(new ValidationResult());
        var expectedErrors = new List<ValidationError>
        {
            new(nameof(user.Role), "Only admins can change user`s role"),
            new(nameof(user.Username), "User with this username already exists"),
            new(nameof(user.Email), "You cannot change email"),
            new(nameof(user.Password), "To change password use action 'Users/ChangePassword'")
        };

        var result = await _sut.ValidateAsync(user);

        result.Should().BeEquivalentTo(expectedErrors);
    }

    [Fact]
    public async Task Validate_ShouldNotAddError_WhenUserIsAdminOrGuest()
    {
        var user = new UserDto { Id = 10, Role = UserRole.Admin };
        _dataAccess.SetReturnById(10, new User { Role = UserRole.Guest });
        CreateValidData();

        var result = await _sut.ValidateAsync(user);

        result.Should().BeEmpty();
    }

    [Fact]
    public async Task Validate_ShouldAddError_WhenStudentNotFound()
    {
        var user = new UserDto { Id = 10, Role = UserRole.Student, StudentId = 15 };
        _dataAccess.SetReturnById(10, new User { Role = UserRole.Student });
        _dataAccess.SetReturnNullById<Student>(15);
        CreateValidData();
        var expected = new List<ValidationError>
            { new(nameof(UserDto.StudentId), "Student with id 15 does not exist") };

        var result = await _sut.ValidateAsync(user);

        result.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public async Task Validate_ShouldAddError_WhenStudentEmailNotNull()
    {
        var user = new UserDto { Id = 10, Role = UserRole.Student, StudentId = 15 };
        _dataAccess.SetReturnById(10, new User { Role = UserRole.Student });
        _dataAccess.SetReturnById(15, new Student { UserEmail = "not@null.email" });
        CreateValidData();
        var expected = new List<ValidationError>
        {
            new(nameof(UserDto.StudentId),
                "Cannot bind to student with id 15 because it is already bind to user with email not@null.email")
        };

        var result = await _sut.ValidateAsync(user);

        result.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public async Task Validate_ShouldNotAddError_WhenStudentEmailIsSameAsUsers()
    {
        var user = new UserDto { Id = 10, Role = UserRole.Student, StudentId = 15, Email = "not@null.email" };
        _dataAccess.SetReturnById(10, new User { Role = UserRole.Student, Email = "not@null.email" });
        _dataAccess.SetReturnById(15, new Student { UserEmail = "not@null.email" });
        CreateValidData();

        var result = await _sut.ValidateAsync(user);

        result.Should().BeEmpty();
    }

    [Fact]
    public async Task Validate_ShouldAddError_WhenTeacherNotFound()
    {
        var user = new UserDto { Id = 10, Role = UserRole.Teacher, TeacherId = 15 };
        _dataAccess.SetReturnById(10, new User { Role = UserRole.Student });
        _dataAccess.SetReturnNullById<Teacher>(15);
        CreateValidData();
        var expected = new List<ValidationError>
            { new(nameof(UserDto.TeacherId), "Teacher with id 15 does not exist") };

        var result = await _sut.ValidateAsync(user);

        result.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public async Task Validate_ShouldAddError_WhenTeacherEmailNotNull()
    {
        var user = new UserDto { Id = 10, Role = UserRole.Teacher, TeacherId = 15 };
        _dataAccess.SetReturnById(10, new User { Role = UserRole.Student });
        _dataAccess.SetReturnById(15, new Teacher { UserEmail = "not@null.email" });
        CreateValidData();
        var expected = new List<ValidationError>
        {
            new(nameof(UserDto.TeacherId),
                "Cannot bind to teacher with id 15 because it is already bind to user with email not@null.email")
        };

        var result = await _sut.ValidateAsync(user);

        result.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public async Task Validate_ShouldNotAddError_WhenTeacherEmailIsSameAsUsers()
    {
        var user = new UserDto { Id = 10, Role = UserRole.Teacher, TeacherId = 15, Email = "not@null.email" };
        _dataAccess.SetReturnById(10, new User { Role = UserRole.Teacher, Email = "not@null.email" });
        _dataAccess.SetReturnById(15, new Teacher { UserEmail = "not@null.email" });
        CreateValidData();

        var result = await _sut.ValidateAsync(user);

        result.Should().BeEmpty();
    }

    [Fact]
    public async Task Validate_ShouldAddError_WhenRoleIsOutOFRange()
    {
        var user = new UserDto { Id = 10, Role = (UserRole)10, TeacherId = 15, Email = "not@null.email" };
        _dataAccess.SetReturnById(10, new User { Role = UserRole.Teacher, Email = "not@null.email" });
        _dataAccess.SetReturnById(15, new Teacher { UserEmail = "not@null.email" });
        CreateValidData();
        var expected = new List<ValidationError>
        {
            new(nameof(UserDto.Role),
                $"No such role found '10'")
        };

        var result = await _sut.ValidateAsync(user);

        result.Should().BeEquivalentTo(expected);
    }

    private void CreateValidData()
    {
        var userByUsername = new DbSetMock<User>().Object;
        _dataAccess.GetByParameters<User, UserUsernameParameters>(
            Arg.Any<UserUsernameParameters>()).Returns(userByUsername);
        _user.FindFirst(CustomClaimTypes.Id).Returns(new Claim("", "15"));
        _user.FindFirst(ClaimTypes.Role).Returns(new Claim("", nameof(UserRole.Admin)));
        _baseValidator.ValidateAsync(Arg.Any<UserDto>()).Returns(new ValidationResult());
    }
}