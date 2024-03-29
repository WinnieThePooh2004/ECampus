﻿using System.IdentityModel.Tokens.Jwt;
using ECampus.DataAccess.Contracts.DataAccess;
using ECampus.DataAccess.Contracts.DataSelectParameters;
using ECampus.Domain.Auth;
using ECampus.Domain.DataTransferObjects;
using ECampus.Domain.Entities;
using ECampus.Domain.Enums;
using ECampus.Domain.Exceptions.DomainExceptions;
using ECampus.Domain.Extensions;
using ECampus.Domain.Responses.Auth;
using ECampus.Services.Contracts.Services;
using ECampus.Services.Services.Auth;
using ECampus.Tests.Shared;
using ECampus.Tests.Shared.Mocks.EntityFramework;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;

namespace ECampus.Tests.Unit.Tests.BackEnd.Domain.Auth;

public class AuthorizationServiceTests
{
    private readonly AuthorizationService _sut;
    private readonly IHttpContextAccessor _httpContextAccessor = Substitute.For<IHttpContextAccessor>();
    private readonly HttpContext _httpContext = Substitute.For<HttpContext>();
    private readonly JwtAuthOptions _authOptions = AuthData.DefaultOptions;

    private readonly IDataAccessFacade
        _parametersDataAccess = Substitute.For<IDataAccessFacade>();

    public AuthorizationServiceTests()
    {
        _httpContextAccessor.HttpContext.Returns(_httpContext);
        _sut = new AuthorizationService(_httpContextAccessor, _authOptions, _parametersDataAccess, Substitute.For<IUserProfileService>());
    }

    [Fact]
    public async Task Login_ShouldThrowException_WhenHttpContextIsNull()
    {
        _httpContextAccessor.HttpContext = null;
        await new Func<Task>(() => _sut.Login(new LoginDto())).Should().ThrowAsync<HttpContextNotFoundExceptions>();
    }

    [Fact]
    public async Task Login_ShouldThrowException_IfPasswordsDontMatch()
    {
        var login = new LoginDto { Password = "password1", Email = "secretEmail@abc.com" };
        var set = new DbSetMock<User>(new User { Password = "invalid password" }).Object;
        _parametersDataAccess
            .GetByParameters<User, UserEmailParameters>(Arg.Is<UserEmailParameters>(p =>
                p.Email == "secretEmail@abc.com")).Returns(set);

        await new Func<Task>(() => _sut.Login(login)).Should().ThrowAsync<DomainException>()
            .WithMessage("Wrong password or email\nError code: 400");
    }

    [Fact]
    public async Task Login_ShouldReturnToken_WhenNoExceptionsThrown()
    {
        var login = new LoginDto { Password = "password2", Email = "secretEmail@abc.com" };
        var user = new User
        {
            Password = login.Password,
            Email = login.Email,
            Id = 10,
            Role = UserRole.Admin
        };
        var set = new DbSetMock<User>(user).Object;
        _parametersDataAccess
            .GetByParameters<User, UserEmailParameters>(Arg.Is<UserEmailParameters>(p =>
                p.Email == "secretEmail@abc.com")).Returns(set);
        var loginResult = new LoginResponse
        {
            Email = user.Email,
            Role = user.Role.ToString(),
            UserId = user.Id,
            Username = user.Username
        };
        loginResult.Token = CreateExpectedToken(loginResult);

        var actual = await _sut.Login(login);

        actual.Should().BeEquivalentTo(actual);
    }

    private string CreateExpectedToken(LoginResponse loginResponse)
    {
        var jwt = new JwtSecurityToken(
            issuer: _authOptions.Issuer,
            audience: _authOptions.Audience,
            claims: loginResponse.CreateClaims(),
            expires: DateTime.UtcNow.Add(TimeSpan.FromMinutes(2)),
            signingCredentials: new SigningCredentials(_authOptions.GetSymmetricSecurityKey(),
                SecurityAlgorithms.HmacSha256));
        return new JwtSecurityTokenHandler().WriteToken(jwt);
    }
}