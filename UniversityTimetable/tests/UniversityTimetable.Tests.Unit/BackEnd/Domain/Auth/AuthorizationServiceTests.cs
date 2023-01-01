using AutoMapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using UniversityTimetable.Domain.Auth;
using UniversityTimetable.Domain.Mapping;
using UniversityTimetable.Shared.DataTransferObjects;
using UniversityTimetable.Shared.Exceptions.DomainExceptions;
using UniversityTimetable.Shared.Interfaces.Auth;
using UniversityTimetable.Shared.Models;
using IAuthenticationService = Microsoft.AspNetCore.Authentication.IAuthenticationService;

namespace UniversityTimetable.Tests.Unit.BackEnd.Domain.Auth;

public class AuthorizationServiceTests
{
    private readonly AuthorizationService _sut;
    private readonly IAuthorizationRepository _repository;
    private readonly IHttpContextAccessor _httpContextAccessor = Substitute.For<IHttpContextAccessor>();
    private readonly HttpContext _httpContext = Substitute.For<HttpContext>();

    public AuthorizationServiceTests()
    {
        _repository = Substitute.For<IAuthorizationRepository>();
        _httpContextAccessor.HttpContext.Returns(_httpContext);
        var mapper = new MapperConfiguration(cfg => cfg.AddProfile<UserProfile>())
            .CreateMapper();
        _sut = new AuthorizationService(_repository, Substitute.For<ILogger<AuthorizationService>>(), mapper,
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
        _repository.GetByEmailAsync("secretEmail@abc.com").Returns(new User { Password = "password2" });

        await new Func<Task>(() => _sut.Login(login)).Should().ThrowAsync<DomainException>()
            .WithMessage("Wrong password or email\nError code: 400");
    }

    // [Fact]
    // public async Task Login_ShouldLogin_WhenNoExceptionsThrown()
    // {
    //     var login = new LoginDto { Password = "password2", Email = "secretEmail@abc.com" };
    //     var user = new UserDto
    //     {
    //         Password = login.Password,
    //         Email = login.Email,
    //         Id = 10,
    //         Role = UserRole.Admin
    //     };
    //     _repository.GetByEmailAsync("secretEmail@abc.com").Returns(_mapper.Map<User>(user));
    //     var expectedClaims = new List<Claim>
    //     {
    //         new(ClaimTypes.Email, user.Email),
    //         new(ClaimTypes.Name, user.Username),
    //         new(ClaimTypes.Role, user.Role.ToString()),
    //         new(CustomClaimTypes.Id, user.Id.ToString(), ClaimValueTypes.Integer32)
    //     };
    //     var requestServices = Substitute.For<IServiceProvider>();
    //     var microsoftAuthenticationService = Substitute.For<IAuthenticationService>();
    //     _httpContext.RequestServices.Returns(requestServices);
    //     requestServices.GetService(typeof(IAuthenticationService)).Returns(microsoftAuthenticationService);
    //     var claims = new List<Claim>();
    //     microsoftAuthenticationService.SignInAsync(_httpContext, CookieAuthenticationDefaults.AuthenticationScheme,
    //             Arg.Do<ClaimsPrincipal>(u => claims = u.Claims.ToList()), Arg.Any<AuthenticationProperties>())
    //         .Returns(Task.CompletedTask);
    //
    //     await _sut.Login(login);
    //     await microsoftAuthenticationService.Received(1).SignInAsync(_httpContext,
    //         CookieAuthenticationDefaults.AuthenticationScheme, Arg.Any<ClaimsPrincipal>(),
    //         Arg.Any<AuthenticationProperties>());
    //
    //     expectedClaims
    //         .All(claim =>
    //             claims.Should().ContainEquivalentOf(claim, 
    //                 opt => opt.ComparingByMembers<Claim>()) is not null).Should()
    //         .Be(true);
    // }

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