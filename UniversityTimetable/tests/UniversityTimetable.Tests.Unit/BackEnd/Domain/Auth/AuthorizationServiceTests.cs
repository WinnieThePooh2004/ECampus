using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using UniversityTimetable.Domain.Auth;
using UniversityTimetable.Shared.Auth;
using UniversityTimetable.Shared.DataTransferObjects;
using UniversityTimetable.Shared.Enums;
using UniversityTimetable.Shared.Exceptions.DomainExceptions;
using UniversityTimetable.Shared.Interfaces.Auth;
using UniversityTimetable.Shared.Models;
using UniversityTimetable.Tests.Shared.DataFactories;
using IAuthenticationService = Microsoft.AspNetCore.Authentication.IAuthenticationService;

namespace UniversityTimetable.Tests.Unit.BackEnd.Domain.Auth;

public class AuthorizationServiceTests
{
    private readonly AuthorizationService _sut;
    private readonly IAuthorizationDataAccess _dataAccess;
    private readonly IHttpContextAccessor _httpContextAccessor = Substitute.For<IHttpContextAccessor>();
    private readonly HttpContext _httpContext = Substitute.For<HttpContext>();
    private readonly IMapper _mapper = MapperFactory.Mapper;

    public AuthorizationServiceTests()
    {
        _dataAccess = Substitute.For<IAuthorizationDataAccess>();
        _httpContextAccessor.HttpContext.Returns(_httpContext);
        _sut = new AuthorizationService(_dataAccess, _mapper,
            _httpContextAccessor);
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
    public async Task Login_ShouldLogin_WhenNoExceptionsThrown()
    {
        var login = new LoginDto { Password = "password2", Email = "secretEmail@abc.com" };
        var user = new UserDto
        {
            Password = login.Password,
            PasswordConfirm = login.Password,
            Email = login.Email,
            Id = 10,
            Role = UserRole.Admin,
            SavedAuditories = new List<AuditoryDto>(),
            SavedGroups = new List<GroupDto>(),
            SavedTeachers = new List<TeacherDto>()
        };
        _dataAccess.GetByEmailAsync("secretEmail@abc.com").Returns(_mapper.Map<User>(user));
        var requestServices = Substitute.For<IServiceProvider>();
        var microsoftAuthenticationService = Substitute.For<IAuthenticationService>();
        _httpContext.RequestServices.Returns(requestServices);
        requestServices.GetService(typeof(IAuthenticationService)).Returns(microsoftAuthenticationService);
        var claims = new List<Claim>();
        microsoftAuthenticationService.SignInAsync(_httpContext, CookieAuthenticationDefaults.AuthenticationScheme,
                Arg.Do<ClaimsPrincipal>(u => claims = u.Claims.ToList()), Arg.Any<AuthenticationProperties>())
            .Returns(Task.CompletedTask);
    
        var result = await _sut.Login(login);
        
        await microsoftAuthenticationService.Received(1).SignInAsync(_httpContext,
            CookieAuthenticationDefaults.AuthenticationScheme, Arg.Any<ClaimsPrincipal>(),
            Arg.Any<AuthenticationProperties>());
        claims.Should().Contain(claim => claim.Type == ClaimTypes.Email && claim.Value == user.Email);
        claims.Should().Contain(claim => claim.Type == ClaimTypes.Role && claim.Value == user.Role.ToString());
        claims.Should().Contain(claim => claim.Type == ClaimTypes.Name && claim.Value == user.Username);
        claims.Should().Contain(claim => claim.Type == CustomClaimTypes.Id && claim.Value == user.Id.ToString());
        result.Should().BeEquivalentTo(user, opt => opt.ComparingByMembers<UserDto>());
    }

    [Fact]
    public async Task Logout_ShouldThrowException_WhenHttpContextIsNull()
    {
        _httpContextAccessor.HttpContext = null;
        await new Func<Task>(() => _sut.Logout()).Should().ThrowAsync<HttpContextNotFoundExceptions>();
    }

    [Fact]
    public async Task Logout_ShouldLogoutInService()
    {
        var requestServices = Substitute.For<IServiceProvider>();
        var microsoftAuthenticationService = Substitute.For<IAuthenticationService>();
        _httpContext.RequestServices.Returns(requestServices);
        requestServices.GetService(typeof(IAuthenticationService)).Returns(microsoftAuthenticationService);

        await _sut.Logout();
        
        await microsoftAuthenticationService.Received(1).SignOutAsync(_httpContext,
            CookieAuthenticationDefaults.AuthenticationScheme, Arg.Any<AuthenticationProperties>());
    }
}