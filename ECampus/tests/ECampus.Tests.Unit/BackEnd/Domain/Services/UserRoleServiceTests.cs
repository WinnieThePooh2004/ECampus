﻿using AutoMapper;
using ECampus.Contracts.DataAccess;
using ECampus.Contracts.DataSelectParameters;
using ECampus.Services.Services;
using ECampus.Shared.DataTransferObjects;
using ECampus.Shared.Enums;
using ECampus.Shared.Models;
using ECampus.Tests.Shared.DataFactories;
using ECampus.Tests.Shared.Mocks.EntityFramework;
using Microsoft.EntityFrameworkCore;

namespace ECampus.Tests.Unit.BackEnd.Domain.Services;

public class UserRoleServiceTests
{
    private readonly UserRolesService _sut;
    private readonly IMapper _mapper = MapperFactory.Mapper;
    private readonly IDataAccessManager _dataAccessManager = Substitute.For<IDataAccessManager>();

    private readonly IDataAccessManager
        _parametersDataAccess = Substitute.For<IDataAccessManager>();

    public UserRoleServiceTests()
    {
        var factory = Substitute.For<IDataAccessManagerFactory>();
        factory.Complex.Returns(_dataAccessManager);
        factory.Primitive.Returns(_parametersDataAccess);
        _sut = new UserRolesService(_mapper, factory);
    }

    [Fact]
    public async Task GetById_ShouldReturnDataAccess_WhenDataAccessReturnsSingle()
    {
        var user = new User { Id = 128 };
        var data = (DbSet<User>)new DbSetMock<User>(new List<User> { user });
        _dataAccessManager.GetByParameters<User, UserRolesParameters>(Arg.Any<UserRolesParameters>())
            .Returns(data);

        var result = await _sut.GetByIdAsync(1);

        result.Id.Should().Be(128);
    }

    [Fact]
    public async Task Create_ShouldSetCorrectTeacherId_WhenUserRoleIsTeacher()
    {
        var teacher = new TeacherDto { Id = 1 };
        var user = new UserDto { Role = UserRole.Teacher, Teacher = teacher, Email = "email" };
        _dataAccessManager.CreateAsync(Arg.Any<User>()).Returns(_mapper.Map<User>(user));

        var result = await _sut.CreateAsync(user);

        result.TeacherId.Should().Be(1);
        await _dataAccessManager.Received(1).UpdateAsync(Arg.Is<Teacher>(s => s.UserEmail == "email"));
        await _dataAccessManager.Received(1).SaveChangesAsync();
    }

    [Fact]
    public async Task Create_ShouldSetCorrectStudentId_WhenUserRoleIsStudent()
    {
        var student = new StudentDto { Id = 1 };
        var user = new UserDto { Role = UserRole.Student, Student = student, Email = "email" };
        _dataAccessManager.CreateAsync(Arg.Any<User>()).Returns(_mapper.Map<User>(user));

        var result = await _sut.CreateAsync(user);

        result.StudentId.Should().Be(1);
        await _dataAccessManager.Received(1).UpdateAsync(Arg.Is<Student>(s => s.UserEmail == "email"));
        await _dataAccessManager.Received(1).SaveChangesAsync();
    }

    [Theory]
    [InlineData(UserRole.Guest)]
    [InlineData(UserRole.Admin)]
    public async Task Create_ShouldJustCreate_WhenRoleIsAdminOrGuest(UserRole role)
    {
        var user = new UserDto { Role = role, Id = 407 };
        _dataAccessManager.CreateAsync(Arg.Any<User>()).Returns(_mapper.Map<User>(user));

        var result = await _sut.CreateAsync(user);

        result.Id.Should().Be(407);
        await _dataAccessManager.Received(1).CreateAsync(Arg.Any<User>());
        await _dataAccessManager.Received(1).SaveChangesAsync();
    }

    [Theory]
    [InlineData(UserRole.Guest)]
    [InlineData(UserRole.Admin)]
    public async Task Update_ShouldClearAllData_WhenUserRoleIsAdminOrGuest(UserRole role)
    {
        var teacher = new TeacherDto { UserEmail = "email", Id = 10 };
        var student = new StudentDto { UserEmail = "email", Id = 10 };
        var user = new UserDto
        {
            Student = student, Teacher = teacher, Role = role, Id = 10, Email = "email", StudentId = 10, TeacherId = 10
        };
        _dataAccessManager.UpdateAsync(Arg.Any<User>()).Returns(_mapper.Map<User>(user));

        await _sut.UpdateAsync(user);

        await _dataAccessManager.Received()
            .UpdateAsync(Arg.Is<User>(s => s.StudentId == null));
        await _dataAccessManager.Received()
            .UpdateAsync(Arg.Is<User>(s => s.TeacherId == null));
    }

    [Fact]
    public async Task Update_ShouldClearStudentData_WhenRoleIsTeacherAndStudentDataNotNull()
    {
        var teacher = new TeacherDto { Id = 10 };
        var student = new StudentDto { UserEmail = "email", Id = 10 };
        var user = new UserDto
        {
            Student = student, Teacher = teacher, Role = UserRole.Teacher, Id = 10, Email = "email", StudentId = 10
        };
        _dataAccessManager.UpdateAsync(Arg.Any<User>()).Returns(_mapper.Map<User>(user));

        var result = await _sut.UpdateAsync(user);

        result.StudentId.Should().BeNull();
        result.TeacherId.Should().Be(10);
        await _dataAccessManager.Received()
            .UpdateAsync(Arg.Is<User>(s => s.StudentId == null && s.Teacher!.UserEmail == "email"));
    }

    [Fact]
    public async Task Update_ShouldClearTeacherData_WhenRoleIsStudentAndTeacherDataNotNull()
    {
        var teacher = new TeacherDto { UserEmail = "email", Id = 10 };
        var student = new StudentDto { Id = 10 };
        var user = new UserDto
        {
            Student = student, Teacher = teacher, Role = UserRole.Student, Id = 10, Email = "email", TeacherId = 10
        };
        _dataAccessManager.UpdateAsync(Arg.Any<User>()).Returns(_mapper.Map<User>(user));

        var result = await _sut.UpdateAsync(user);

        result.TeacherId.Should().BeNull();
        result.StudentId.Should().Be(10);
        await _dataAccessManager.Received()
            .UpdateAsync(Arg.Is<User>(s => s.TeacherId == null && s.Student!.UserEmail == "email"));
    }

    [Fact]
    public async Task Update_ShouldNotClearTeacherData_WhenRoleIsStudentAndDataIsNull()
    {
        var user = new UserDto { Role = UserRole.Student };
        _dataAccessManager.UpdateAsync(Arg.Any<User>()).Returns(_mapper.Map<User>(user));

        await _sut.UpdateAsync(user);

        _dataAccessManager.ReceivedCalls().Count().Should().Be(1);
    }

    [Fact]
    public async Task Update_ShouldNotClearStudentData_WhenRoleIsTeacherAndDataIsNull()
    {
        var user = new UserDto { Role = UserRole.Teacher };
        _dataAccessManager.UpdateAsync(Arg.Any<User>()).Returns(_mapper.Map<User>(user));

        await _sut.UpdateAsync(user);

        _dataAccessManager.ReceivedCalls().Count().Should().Be(1);
    }

    [Fact]
    public async Task Delete_ShouldCall_DeleteService()
    {
        var user = new User { Id = 1 };
        _dataAccessManager.DeleteAsync<User>(1).Returns(user);

        var model = await _sut.DeleteAsync(1);

        model.Id.Should().Be(1);
        await _dataAccessManager.Received().DeleteAsync<User>(1);
    }
}