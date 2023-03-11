using System.Diagnostics;
using ECampus.Contracts.DataSelectParameters;
using ECampus.DataAccess.Contracts.DataAccess;
using ECampus.Services.Services;
using ECampus.Shared.DataTransferObjects;
using ECampus.Shared.Enums;
using ECampus.Shared.Models;
using ECampus.Tests.Shared.DataFactories;
using ECampus.Tests.Shared.Mocks.EntityFramework;
using ECampus.Tests.Unit.Extensions;

namespace ECampus.Tests.Unit.Tests.BackEnd.Domain.Services;

public class UserRoleServiceTests
{
    private readonly UserService _sut;
    private readonly IDataAccessFacade _dataAccess = Substitute.For<IDataAccessFacade>();

    public UserRoleServiceTests()
    {
        _sut = new UserService(MapperFactory.Mapper, _dataAccess);
    }

    [Theory]
    [InlineData(UserRole.Guest)]
    [InlineData(UserRole.Admin)]
    public async Task Update_ShouldNotCallDbTwice_WhenRolesNotChangedAndIsAdminOrGuest(UserRole role)
    {
        var selectResult = new User { Role = role }.ToAsyncQueryable();
        _dataAccess.GetByParameters<User, UserRolesParameters>(Arg.Any<UserRolesParameters>())
            .Returns(selectResult);

        await _sut.UpdateAsync(new UserDto { Id = 12, Role = role });

        _dataAccess.ReceivedCalls().Count().Should().Be(2);
    }

    [Fact]
    public async Task Update_ShouldCallDbTwice_WhenRoleIsStudentAndStudentIdNotChanged()
    {
        var selectResult = new User { Role = UserRole.Student, StudentId = 10 }.ToAsyncQueryable();
        _dataAccess.GetByParameters<User, UserRolesParameters>(Arg.Any<UserRolesParameters>())
            .Returns(selectResult);

        await _sut.UpdateAsync(new UserDto { Id = 12, Role = UserRole.Student, StudentId = 10 });

        _dataAccess.ReceivedCalls().Count().Should().Be(2);
    }

    [Fact]
    public async Task Update_ShouldCallDbTwice_WhenRoleIsTeacherAndTeacherIdNotChanged()
    {
        var selectResult = new User { Role = UserRole.Teacher, TeacherId = 10 }.ToAsyncQueryable();
        _dataAccess.GetByParameters<User, UserRolesParameters>(Arg.Any<UserRolesParameters>())
            .Returns(selectResult);

        await _sut.UpdateAsync(new UserDto { Id = 12, Role = UserRole.Teacher, TeacherId = 10 });

        _dataAccess.ReceivedCalls().Count().Should().Be(2);
    }

    [Fact]
    public async Task Update_ShouldUpdateStudentEmail_WhenRoleIsStudentAndStudentIdChanged()
    {
        var oldStudent = new Student { UserEmail = "oldEmail" };
        var userFromDb = new User
            { Role = UserRole.Student, StudentId = 10, Student = oldStudent, Email = "newEmail" };
        var selectResult = userFromDb.ToAsyncQueryable();
        var newStudent = new Student { Id = 1, UserEmail = null };
        _dataAccess.SetReturnById(12, userFromDb);
        _dataAccess.SetReturnById(1, newStudent);
        _dataAccess.GetByParameters<User, UserRolesParameters>(Arg.Any<UserRolesParameters>())
            .Returns(selectResult);

        await _sut.UpdateAsync(new UserDto { Id = 12, Role = UserRole.Student, StudentId = 1 });

        oldStudent.UserEmail.Should().BeNull();
        userFromDb.StudentId.Should().Be(1);
        newStudent.UserEmail.Should().Be(userFromDb.Email);
    }

    [Fact]
    public async Task Update_ShouldUpdateStudentEmail_WhenRoleIsTeacherAndTeacherIdChanged()
    {
        var oldTeacher = new Teacher { UserEmail = "oldEmail" };
        var userFromDb = new User
            { Role = UserRole.Teacher, StudentId = 10, Teacher = oldTeacher, Email = "newEmail" };
        var selectResult = userFromDb.ToAsyncQueryable();
        var newTeacher = new Teacher { Id = 1, UserEmail = null };
        _dataAccess.SetReturnById(12, userFromDb);
        _dataAccess.SetReturnById(1, newTeacher);
        _dataAccess.GetByParameters<User, UserRolesParameters>(Arg.Any<UserRolesParameters>())
            .Returns(selectResult);

        await _sut.UpdateAsync(new UserDto { Id = 12, Role = UserRole.Teacher, TeacherId = 1 });

        oldTeacher.UserEmail.Should().BeNull();
        userFromDb.TeacherId.Should().Be(1);
        newTeacher.UserEmail.Should().Be(userFromDb.Email);
    }

    [Fact]
    public async Task Update_ShouldThrowException_WhenRoleNotChangedAndIsOutOfRange()
    {
        var userFromDb = new User { Role = (UserRole)10 }.ToAsyncQueryable();
        _dataAccess.GetByParameters<User, UserRolesParameters>(Arg.Any<UserRolesParameters>())
            .Returns(userFromDb);

        await new Func<Task>(() => _sut.UpdateAsync(new UserDto { Role = (UserRole)10 })).Should()
            .ThrowAsync<UnreachableException>()
            .WithInnerExceptionExactly<UnreachableException, ArgumentOutOfRangeException>();
    }

    [Theory]
    [InlineData(UserRole.Admin)]
    [InlineData(UserRole.Guest)]
    public async Task Update_ShouldClearStudentAndTeacherData_WhenRoleIsAdminOrGuest(UserRole role)
    {
        var teacher = new Teacher { UserEmail = "email" };
        var student = new Student { UserEmail = "email" };
        var userFromDb = new User { Student = student, Teacher = teacher, Role = UserRole.Student };
        var selected = userFromDb.ToAsyncQueryable();
        _dataAccess.GetByParameters<User, UserRolesParameters>(Arg.Any<UserRolesParameters>())
            .Returns(selected);

        await _sut.UpdateAsync(new UserDto { Id = 14, Role = role });

        student.UserEmail.Should().BeNull();
        teacher.UserEmail.Should().BeNull();
        userFromDb.StudentId.Should().BeNull();
        userFromDb.TeacherId.Should().BeNull();
        userFromDb.Teacher.Should().BeNull();
        userFromDb.Student.Should().BeNull();
    }

    [Theory]
    [InlineData(UserRole.Admin)]
    [InlineData(UserRole.Guest)]
    public async Task Update_ShouldNotClearStudentAndTeacherData_WhenRoleIsAdminOrGuestAndRelationsAreNull(
        UserRole role)
    {
        var userFromDb = new User { Role = UserRole.Student };
        var selected = userFromDb.ToAsyncQueryable();
        _dataAccess.GetByParameters<User, UserRolesParameters>(Arg.Any<UserRolesParameters>())
            .Returns(selected);

        await _sut.UpdateAsync(new UserDto { Id = 14, Role = role });

        userFromDb.StudentId.Should().BeNull();
        userFromDb.TeacherId.Should().BeNull();
        userFromDb.Teacher.Should().BeNull();
        userFromDb.Student.Should().BeNull();
    }

    [Fact]
    public async Task Update_ShouldUpdateEmail_WhenRoleChangedToStudent()
    {
        var userFromDb = new User { Role = UserRole.Guest, Email = "email" };
        var student = new Student { Id = 10 };
        var selected = userFromDb.ToAsyncQueryable();
        _dataAccess.GetByParameters<User, UserRolesParameters>(Arg.Any<UserRolesParameters>())
            .Returns(selected);
        _dataAccess.SetReturnById(10, student);

        await _sut.UpdateAsync(new UserDto { Id = 14, Role = UserRole.Student, StudentId = 10 });

        userFromDb.StudentId.Should().Be(10);
        student.UserEmail.Should().Be(userFromDb.Email);
    }

    [Fact]
    public async Task Update_ShouldUpdateEmail_WhenRoleChangedToTeacher()
    {
        var userFromDb = new User { Role = UserRole.Guest, Email = "email" };
        var teacher = new Teacher { Id = 10 };
        var selected = userFromDb.ToAsyncQueryable();
        _dataAccess.GetByParameters<User, UserRolesParameters>(Arg.Any<UserRolesParameters>())
            .Returns(selected);
        _dataAccess.SetReturnById(10, teacher);

        await _sut.UpdateAsync(new UserDto { Id = 14, Role = UserRole.Teacher, TeacherId = 10 });

        userFromDb.TeacherId.Should().Be(10);
        teacher.UserEmail.Should().Be(userFromDb.Email);
    }
    
    [Fact]
    public async Task Update_ShouldThrow_WhenRoleIsOutOfRange()
    {
        var userFromDb = new User { Role = (UserRole)1, Email = "email" };
        var teacher = new Teacher { Id = 10 };
        var selected = userFromDb.ToAsyncQueryable();
        _dataAccess.GetByParameters<User, UserRolesParameters>(Arg.Any<UserRolesParameters>())
            .Returns(selected);
        _dataAccess.SetReturnById(10, teacher);

        await new Func<Task>(() => _sut.UpdateAsync(new UserDto { Role = (UserRole)10 })).Should()
            .ThrowAsync<UnreachableException>()
            .WithInnerExceptionExactly<UnreachableException, ArgumentOutOfRangeException>();
    }
    
    [Fact]
    public async Task Update_ShouldThrow_WhenRoleAreEqualAndIsOutOfRange()
    {
        var userFromDb = new User { Role = (UserRole)10, Email = "email" };
        var teacher = new Teacher { Id = 10 };
        var selected = userFromDb.ToAsyncQueryable();
        _dataAccess.GetByParameters<User, UserRolesParameters>(Arg.Any<UserRolesParameters>())
            .Returns(selected);
        _dataAccess.SetReturnById(10, teacher);

        await new Func<Task>(() => _sut.UpdateAsync(new UserDto { Role = (UserRole)10 })).Should()
            .ThrowAsync<UnreachableException>()
            .WithInnerExceptionExactly<UnreachableException, ArgumentOutOfRangeException>();
    }

    [Fact]
    public async Task GetById_ShouldReturnFromDataAccess()
    {
        var user = new User().ToAsyncQueryable();
        _dataAccess.GetByParameters<User, UserRolesParameters>(Arg.Any<UserRolesParameters>()).Returns(user);

        await _sut.GetByIdAsync(10);

        _dataAccess.Received(1)
            .GetByParameters<User, UserRolesParameters>(Arg.Is<UserRolesParameters>(u => u.UserId == 10));
    }

    [Fact]
    public async Task Delete_ShouldDeleteFromDb_WhenUserExist()
    {
        var user = new User();
        var userQuery = user.ToAsyncQueryable();
        _dataAccess.GetByParameters<User, UserRolesParameters>(Arg.Any<UserRolesParameters>()).Returns(userQuery);

        await _sut.DeleteAsync(10);

        _dataAccess.Received(1).Delete(user);
    }

    [Theory]
    [InlineData(UserRole.Admin)]
    [InlineData(UserRole.Guest)]
    public async Task Create_ShouldOnlyCreateAndSave_WhenRoleIsAdminOrGuest(UserRole role)
    {
        var user = new UserDto
            { Role = role, StudentId = 0, Student = new StudentDto(), Teacher = new TeacherDto(), TeacherId = 0 };

        var result = await _sut.CreateAsync(user);

        _dataAccess.ReceivedCalls().Count().Should().Be(2);
        result.StudentId.Should().BeNull();
        result.Student.Should().BeNull();
        result.Teacher.Should().BeNull();
        result.TeacherId.Should().BeNull();
    }

    [Fact]
    public async Task Create_ShouldAddTeacher_WhenRoleIsTeacher()
    {
        var teacher = new Teacher { Id = 19, FirstName = "needed teacher" };
        _dataAccess.SetReturnById(19, teacher);

        var result = await _sut.CreateAsync(new UserDto
            { TeacherId = 19, Role = UserRole.Teacher, Student = new StudentDto(), StudentId = 9 });

        result.Student.Should().BeNull();
        result.StudentId.Should().BeNull();
        result.Teacher!.Id.Should().Be(19);
    }

    [Fact]
    public async Task Create_ShouldAddStudent_WhenRoleIsStudent()
    {
        var student = new Student { Id = 19, FirstName = "needed teacher" };
        _dataAccess.SetReturnById(19, student);

        var result = await _sut.CreateAsync(new UserDto
            { StudentId = 19, Role = UserRole.Student, Teacher = new TeacherDto(), TeacherId = 9 });

        result.Teacher.Should().BeNull();
        result.TeacherId.Should().BeNull();
        result.Student!.Id.Should().Be(19);
    }

    [Fact]
    public async Task Create_ShouldThrowException_WhenRoleIsOutOfEnum()
    {
        await new Func<Task>(() => _sut.CreateAsync(new UserDto { Role = (UserRole)10 })).Should()
            .ThrowAsync<UnreachableException>()
            .WithInnerExceptionExactly<UnreachableException, ArgumentOutOfRangeException>();
    }
}