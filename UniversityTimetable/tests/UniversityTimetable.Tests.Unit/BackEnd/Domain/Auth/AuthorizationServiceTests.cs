using System.IdentityModel.Tokens.Jwt;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using UniversityTimetable.Domain.Auth;
using UniversityTimetable.Shared.Auth;
using UniversityTimetable.Shared.DataTransferObjects;
using UniversityTimetable.Shared.Enums;
using UniversityTimetable.Shared.Exceptions.DomainExceptions;
using UniversityTimetable.Shared.Extensions;
using UniversityTimetable.Shared.Interfaces.Auth;
using UniversityTimetable.Shared.Models;
using UniversityTimetable.Tests.Shared;
using UniversityTimetable.Tests.Shared.DataFactories;

namespace UniversityTimetable.Tests.Unit.BackEnd.Domain.Auth;

public class AuthorizationServiceTests
{
    private readonly AuthorizationService _sut;
    private readonly IAuthorizationDataAccess _dataAccess;
    private readonly IHttpContextAccessor _httpContextAccessor = Substitute.For<IHttpContextAccessor>();
    private readonly HttpContext _httpContext = Substitute.For<HttpContext>();
    private readonly IMapper _mapper = MapperFactory.Mapper;
    private readonly JwtAuthOptions _authOptions = AuthData.DefaultOptions;

    public AuthorizationServiceTests()
    {
        _dataAccess = Substitute.For<IAuthorizationDataAccess>();
        _httpContextAccessor.HttpContext.Returns(_httpContext);
        _sut = new AuthorizationService(_dataAccess, _mapper,
            _httpContextAccessor, _authOptions);
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
        _dataAccess.GetByEmailAsync("secretEmail@abc.com").Returns(new User { Password = "password2" });

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
        _dataAccess.GetByEmailAsync("secretEmail@abc.com").Returns(user);
        var loginResult = new LoginResult
        {
            Email = user.Email,
            Role = user.Role,
            UserId = user.Id,
            Username = user.Username
        };
        loginResult.Token = CreateExpectedToken(loginResult);

        var actual = await _sut.Login(login);

        actual.Should().BeEquivalentTo(actual);
    }

    private string CreateExpectedToken(LoginResult loginResult)
    {
        var jwt = new JwtSecurityToken(
            issuer: _authOptions.Issuer,
            audience: _authOptions.Audience,
            claims: HttpContextExtensions.CreateClaims(loginResult),
            expires: DateTime.UtcNow.Add(TimeSpan.FromMinutes(2)),
            signingCredentials: new SigningCredentials(_authOptions.GetSymmetricSecurityKey(),
                SecurityAlgorithms.HmacSha256));
        return new JwtSecurityTokenHandler().WriteToken(jwt);
    }
}