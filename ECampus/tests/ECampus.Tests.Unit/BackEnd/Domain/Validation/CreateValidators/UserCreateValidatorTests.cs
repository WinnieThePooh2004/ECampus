﻿using System.Security.Claims;
using ECampus.Contracts.DataAccess;
using ECampus.Contracts.DataSelectParameters;
using ECampus.Domain.Interfaces.Validation;
using ECampus.Domain.Validation.CreateValidators;
using ECampus.Shared.DataTransferObjects;
using ECampus.Shared.Enums;
using ECampus.Shared.Models;
using ECampus.Shared.Validation;
using ECampus.Tests.Shared.Mocks.EntityFramework;
using Microsoft.AspNetCore.Http;

namespace ECampus.Tests.Unit.BackEnd.Domain.Validation.CreateValidators;

public class UserCreateValidatorTests
{
    private readonly UserCreateValidator _sut;
    private readonly ICreateValidator<UserDto> _baseValidator = Substitute.For<ICreateValidator<UserDto>>();
    private readonly ClaimsPrincipal _user = Substitute.For<ClaimsPrincipal>();
    private readonly IDataAccessManager _dataAccess = Substitute.For<IDataAccessManager>();

    public UserCreateValidatorTests()
    {
        var accessor = Substitute.For<IHttpContextAccessor>();
        accessor.HttpContext = Substitute.For<HttpContext>();
        accessor.HttpContext.User = _user;
        _sut = new UserCreateValidator(_baseValidator, accessor, _dataAccess);
    }

    [Fact]
    public async Task Validate_ShouldNotCallDataAccess_WhenBaseValidatorReturnsErrors()
    {
        var user = new UserDto();
        var baseErrors = new ValidationResult(new ValidationError("", ""));
        _baseValidator.ValidateAsync(user).Returns(baseErrors);

        var actualResult = await _sut.ValidateAsync(user);

        actualResult.Count().Should().Be(1);
    }

    [Fact]
    public async Task Validate_ShouldAddError_WhenValidationErrorsOccured()
    {
        var user = new UserDto { Username = "username", Email = "email" };
        var usernameSelect = new User().ToAsyncQueryable();
        var emailSelect = new User().ToAsyncQueryable();
        _dataAccess.GetByParameters<User, UserUsernameParameters>(Arg.Any<UserUsernameParameters>())
            .Returns(usernameSelect);
        _dataAccess.GetByParameters<User, UserEmailParameters>(Arg.Any<UserEmailParameters>())
            .Returns(emailSelect);
        _baseValidator.ValidateAsync(user).Returns(new ValidationResult());
        var expected = new List<ValidationError>
        {
            new(nameof(user.Email), "User with same email already exists"),
            new(nameof(user.Username), "User with same username already exists"),
        };

        var result = await _sut.ValidateAsync(user);

        result.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public async Task Validate_ShouldAddError_WhenRoleIsNotGuestAndCurrentUserNotAdmin()
    {
        var user = new UserDto { Username = "username", Email = "email", Role = UserRole.Admin };
        _baseValidator.ValidateAsync(user).Returns(new ValidationResult());
        var expected = new List<ValidationError>
        {
            new(nameof(user.Role), $"Only admin can create user with roles different from {nameof(UserRole.Guest)}")
        };

        var result = await _sut.ValidateAsync(user);

        result.Should().BeEquivalentTo(expected);
    }
}