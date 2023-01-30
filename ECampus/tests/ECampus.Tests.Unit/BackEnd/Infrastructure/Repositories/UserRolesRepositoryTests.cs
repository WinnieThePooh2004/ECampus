using ECampus.Infrastructure;
using ECampus.Infrastructure.Repositories;
using ECampus.Shared.Enums;
using ECampus.Shared.Exceptions.InfrastructureExceptions;
using ECampus.Shared.Models;
using ECampus.Tests.Shared.Mocks.EntityFramework;
using Microsoft.EntityFrameworkCore;

namespace ECampus.Tests.Unit.BackEnd.Infrastructure.Repositories;

public class UserRolesRepositoryTests
{
    private readonly UserRolesRepository _sut;
    private readonly ApplicationDbContext _context = Substitute.For<ApplicationDbContext>();

    public UserRolesRepositoryTests()
    {
        _sut = new UserRolesRepository(_context);
    }

    [Fact]
    public async Task GetById_ShouldReturnFromDb_WhenExistInDb()
    {
        var user = new User { Id = 1 };
        _context.Users = new DbSetMock<User>(user);

        var result = await _sut.GetByIdAsync(1);

        result.Should().Be(user);
    }

    [Fact]
    public async Task GetById_ShouldThrowException_WhenNotFoundInDb()
    {
        _context.Users = new DbSetMock<User>();

        await new Func<Task>(() => _sut.GetByIdAsync(1)).Should()
            .ThrowAsync<ObjectNotFoundByIdException>()
            .WithMessage(new ObjectNotFoundByIdException(typeof(User), 1).Message);
    }

    [Fact]
    public async Task Create_ShouldSetCorrectTeacherId_WhenUserRoleIsTeacher()
    {
        var teacher = new Teacher { Id = 1 };
        var user = new User { Role = UserRole.Teacher, Teacher = teacher, Email = "email" };

        await _sut.CreateAsync(user);

        user.Teacher.Should().BeNull();
        _context.Received(1).Update(teacher);
        teacher.UserEmail = user.Email;
        user.TeacherId.Should().Be(1);
    }

    [Fact]
    public async Task Create_ShouldSetCorrectStudentId_WhenUserRoleIsStudent()
    {
        var student = new Student { Id = 1 };
        var user = new User { Role = UserRole.Student, Student = student, Email = "email" };

        await _sut.CreateAsync(user);

        user.Teacher.Should().BeNull();
        _context.Received(1).Update(student);
        student.UserEmail = user.Email;
        user.StudentId.Should().Be(1);
    }

    [Theory]
    [InlineData(UserRole.Guest)]
    [InlineData(UserRole.Admin)]
    public async Task Create_ShouldJustCreate_WhenRoleIsAdminOrGuest(UserRole role)
    {
        var user = new User { Role = role };

        var result = await _sut.CreateAsync(user);

        result.Should().Be(user);
    }

    [Theory]
    [InlineData(UserRole.Guest)]
    [InlineData(UserRole.Admin)]
    public async Task Update_ShouldClearAllData_WhenUserRoleIsAdminOrGuest(UserRole role)
    {
        var teacher = new Teacher { UserEmail = "email", Id = 10 };
        var student = new Student { UserEmail = "email", Id = 10 };
        var user = new User
        {
            Student = student, Teacher = teacher, Role = role, Id = 10, Email = "email", StudentId = 10, TeacherId = 10
        };
        var expectedUser = new User { Id = 10, Email = "email", Role = role };

        await _sut.UpdateAsync(user);

        teacher.UserEmail.Should().BeNull();
        student.UserEmail.Should().BeNull();
        user.Should().BeEquivalentTo(expectedUser);
    }

    [Fact]
    public async Task Update_ShouldClearStudentData_WhenRoleIsTeacherAndStudentDataNotNull()
    {
        var teacher = new Teacher { Id = 10 };
        var student = new Student { UserEmail = "email", Id = 10 };
        var user = new User
        {
            Student = student, Teacher = teacher, Role = UserRole.Teacher, Id = 10, Email = "email", StudentId = 10
        };

        await _sut.UpdateAsync(user);

        user.StudentId.Should().BeNull();
        user.TeacherId.Should().Be(10);
        teacher.UserEmail.Should().Be(user.Email);
        student.UserEmail.Should().BeNull();
        _context.Received(1).Update(user);
        _context.Received(1).Update(student);
    }

    [Fact]
    public async Task Update_ShouldClearTeacherData_WhenRoleIsStudentAndTeacherDataNotNull()
    {
        var teacher = new Teacher { UserEmail = "email", Id = 10 };
        var student = new Student { Id = 10 };
        var user = new User
        {
            Student = student, Teacher = teacher, Role = UserRole.Student, Id = 10, Email = "email", TeacherId = 10
        };

        await _sut.UpdateAsync(user);

        user.TeacherId.Should().BeNull();
        user.StudentId.Should().Be(10);
        student.UserEmail.Should().Be(user.Email);
        teacher.UserEmail.Should().BeNull();
        _context.Received(1).Update(user);
        _context.Received(1).Update(teacher);
    }

    [Fact]
    public async Task Update_ShouldNotClearTeacherData_WhenRoleIsStudentAndDataIsNull()
    {
        var user = new User { Role = UserRole.Student };

        await _sut.UpdateAsync(user);

        _context.DidNotReceive().Update(user);
    }
    
    [Fact]
    public async Task Update_ShouldNotClearStudentData_WhenRoleIsTeacherAndDataIsNull()
    {
        var user = new User { Role = UserRole.Teacher };

        await _sut.UpdateAsync(user);

        _context.DidNotReceive().Update(user);
    }

    [Fact]
    public async Task Delete_ShouldCall_DeleteService()
    {
        var model = await _sut.DeleteAsync(1);

        model.Id.Should().Be(1);
    }

    [Fact]
    public async Task Delete_ShouldThrowNoFoundByIdException_WhenDbUpdateConcurrencyExceptionThrown()
    {
        _context.SaveChangesAsync().Returns(1).AndDoes(_ => throw new DbUpdateConcurrencyException());

        await new Func<Task>(() => _sut.DeleteAsync(10)).Should()
            .ThrowAsync<ObjectNotFoundByIdException>();
    }

    [Fact]
    public async Task Delete_ShouldThrowUnhandledException_WhenNotDbUpdateConcurrencyExceptionThrown()
    {
        var innerException = new DbUpdateException();
        _context.SaveChangesAsync().Returns(1).AndDoes(_ => throw innerException);

        await new Func<Task>(() => _sut.DeleteAsync(10)).Should()
            .ThrowAsync<UnhandledInfrastructureException>()
            .WithMessage(new UnhandledInfrastructureException(innerException).Message)
            .WithInnerExceptionExactly<UnhandledInfrastructureException, DbUpdateException>();
    }
}